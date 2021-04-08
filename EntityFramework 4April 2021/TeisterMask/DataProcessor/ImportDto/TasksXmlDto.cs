using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class TasksXmlDto
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40,MinimumLength =2)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        [EnumDataType(typeof(ExecutionType))]
        [Required]
        public int ExecutionType { get; set; }


        [XmlElement("LabelType")]
        [EnumDataType(typeof(LabelType))]
        [Required]
        public int LabelType { get; set; }
    }
   // <Tasks>
    //  <Task>
    //    <Name>Australian</Name>
    //    <OpenDate>19/08/2018</OpenDate>
    //    <DueDate>13/07/2019</DueDate>
    //    <ExecutionType>2</ExecutionType>
    //    <LabelType>0</LabelType>
    //  </Task>
}
