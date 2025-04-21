using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleWebAppMVC.Models
{
    public class TaskDbModel : TaskModel
    {
        public TaskDbModel() {}

        public TaskDbModel(TaskModel task)
        {
            this.Update(task);
        }

        public TaskDbModel(TaskModel task, string createdBy)
        {
            this.Update(task);

            this.CreatedBy = createdBy;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        public void Update(TaskModel task)
        {
            this.Title       = task.Title;
            this.Description = task.Description;
            this.Date        = task.Date;
            this.Status      = task.Status;
        }
    }
}
