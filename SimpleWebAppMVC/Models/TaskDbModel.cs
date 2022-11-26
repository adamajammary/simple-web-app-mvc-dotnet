using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleWebAppMVC.Models
{
    public class TaskDbModel : Task
    {
        public TaskDbModel() {}

        public TaskDbModel(Task task)
        {
            this.Update(task);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public void Update(Task task)
        {
            this.Title       = task.Title;
            this.Description = task.Description;
            this.Date        = task.Date;
            this.Status      = task.Status;
        }
    }
}
