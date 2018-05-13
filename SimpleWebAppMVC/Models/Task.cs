using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleWebAppMVC.Models
{
    /**
     * Task Model 
     */
    public class Task
    {
        public string ID { get; set; }

        [StringLength(50, MinimumLength = 3), Required]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public string Status { get; set; }

        [NotMapped]
        public SelectList StatusCodes { get; } = new SelectList(
            new List<string> {
                "N/A", "Not Started", "Started", "In Progress", "Almost Done", "Completed"
            }
        );
        /*public List<SelectListItem> StatusCodes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "N/A",         Text = "N/A" },
            new SelectListItem { Value = "Not Started", Text = "Not Started" },
            new SelectListItem { Value = "Started",     Text = "Started" },
            new SelectListItem { Value = "In Progress", Text = "In Progress" },
            new SelectListItem { Value = "Almost Done", Text = "Almost Done" },
            new SelectListItem { Value = "Completed",   Text = "Completed" }
        };*/
    }
}
