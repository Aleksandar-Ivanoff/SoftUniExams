﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeesImportDto
    {
        [Required]
        [StringLength(40,MinimumLength =3)]
        [RegularExpression("^[A-Za-z0-9]{3,40}$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d{2}-\d{3}-\d{4}")]
        public string Phone { get; set; }

        [Required]
        public int[] Tasks { get; set; }
    }

    

}
