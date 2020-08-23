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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 3), Required]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public string Status { get; set; }

        [NotMapped]
        public SelectList StatusCodes { get; } = new SelectList(new List<string> {
            "N/A", "Not Started", "Started", "In Progress", "Almost Done", "Completed"
        });

        public void Update(Task task)
        {
            this.Title       = task.Title;
            this.Description = task.Description;
            this.Date        = task.Date;
            this.Status      = task.Status;
        }
    }
}
