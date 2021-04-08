namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
   
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            
            XmlSerializer sertializer = new XmlSerializer(typeof(ProjectsExport[]), new XmlRootAttribute("Projects"));

            var projects = context.Projects.ToArray().Where(p => p.Tasks.Any())
                .Select(p => new ProjectsExport
                {
                    ProjectName = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",


                    Tasks = p.Tasks.Select(x => new TasksXmlExport
                    {
                        LabelName = x.LabelType.ToString(),
                        Name = x.Name

                    }).OrderBy(x => x.Name).ToArray()

                }).OrderByDescending(x => x.TasksCount).ThenBy(x => x.ProjectName).ToArray();


            

            var xml = XmlConverter.Serialize(projects, "Projects");

            return xml;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees.ToArray().Where(e=>e.EmployeesTasks.Any(x=>x.Task.OpenDate.Ticks >= date.Ticks))
                .Select(e => new EmployeesJsonExportDto
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Select(t => t.Task).Where(t=>t.OpenDate.Ticks >=date.Ticks).OrderByDescending(x=>x.DueDate)
                    .ThenBy(x=>x.Name)
                    .Select(x=>new TasksDtoExport {
                        TaskName = x.Name,
                        OpenDate = x.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = x.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = x.LabelType.ToString(),
                        ExecutionType = x.ExecutionType.ToString()
                    }).ToList()
                    

                }).OrderByDescending(o => o.Tasks.Count()).ThenBy(o => o.Username).Take(10).ToArray();

            var json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            return json;
        }
    }
}