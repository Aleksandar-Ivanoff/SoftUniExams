using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeesJsonExportDto
    {
        public string Username { get; set; }

        public ICollection<TasksDtoExport> Tasks { get; set; }  // MAY HAVE PROBLEM WITH ICOLLECTION TO IENUMERABLE
    }
}
