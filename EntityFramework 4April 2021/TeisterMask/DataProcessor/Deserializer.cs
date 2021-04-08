namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ProjectsXmlDto[]), new XmlRootAttribute("Projects"));

            var xml = (ProjectsXmlDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var projectDto in xml)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var IsprojectDueDate = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDueDate);
                var project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = IsprojectDueDate? (DateTime?)parsedDueDate : null

                    //isReleasedDateValid ? (DateTime?)releasedTime : null
                };
                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    var taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var taskDueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (taskOpenDate <= project.OpenDate || taskDueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var labelType = (LabelType)taskDto.LabelType;
                    var executionType = (ExecutionType)taskDto.ExecutionType;

                    var task = context.Tasks.FirstOrDefault(x => x.Name == taskDto.Name) ?? new Task { Name = taskDto.Name, OpenDate = taskOpenDate, DueDate = taskDueDate, LabelType = labelType, ExecutionType = executionType };
                    project.Tasks.Add(task);

                  
                }

                context.Projects.Add(project);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported project - {project.Name} with {project.Tasks.Count()} tasks.");
            }
           return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<IEnumerable<EmployeesImportDto>>(jsonString);
            var sb = new StringBuilder();

            foreach (var employeeDto in json)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var tasks = employeeDto.Tasks.Distinct().ToList();

                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone,

                };

                foreach (var t in tasks)
                {
                    var task = context.Tasks.FirstOrDefault(x => x.Id == t);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    employee.EmployeesTasks.Add(new EmployeeTask { Task = task });
                }

                context.Employees.Add(employee);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported employee - {employee.Username} with {employee.EmployeesTasks.Count()} tasks.");
            }
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}