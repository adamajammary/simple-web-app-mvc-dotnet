using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleWebAppMVC.Validation;

namespace SimpleWebAppMVC.Models
{
    public class Task
    {
        /// <example>Task #1</example>
        [StringLength(50, MinimumLength = 3), Required]
        public string Title { get; set; }

        /// <example>The first task</example>
        [StringLength(250)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        /// <summary>Valid status codes: [ "N/A", "Not Started", "Started", "In Progress", "Almost Done", "Completed" ]</summary>
        /// <example>N/A</example>
        [ValidateStatusCodes, Required]
        public string Status { get; set; }

        [NotMapped]
        internal SelectList StatusCodes { get; } = new SelectList(ValidateStatusCodesAttribute.ValidStatusCodes);
    }
}
