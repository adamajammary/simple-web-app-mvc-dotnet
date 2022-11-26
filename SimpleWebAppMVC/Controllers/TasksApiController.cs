using Microsoft.AspNetCore.Mvc;
using SimpleWebAppMVC.Data;
using System.Linq;

namespace SimpleWebAppMVC.Controllers
{
    [Produces("application/json")]
    [Route("api/Tasks")]
    public class TasksApiController : Controller
    {
        private readonly AppDbContext dbContext;

        public TasksApiController(AppDbContext dbCtx)
        {
            this.dbContext = dbCtx;
        }

        /// <summary>Returns a list of all the tasks</summary>
        [HttpGet]
        public JsonResult Get()
        {
            return Json(from task in this.dbContext.Tasks select task);
        }

        /// <summary>Returns the task with the specified ID if it exists</summary>
        /// <param name="id">Task ID</param>
        [HttpGet("{id}", Name = "GetTask")]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = this.dbContext.Tasks.SingleOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            return Json(task);
        }

        /// <summary>Adds a new task</summary>
        /// <param name="newTask">New task</param>
        [HttpPost]
        public IActionResult Post([FromBody] Models.Task newTask)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var task = new Models.TaskDbModel(newTask);

            this.dbContext.Add(task);
            this.dbContext.SaveChanges();

            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }

        /// <summary>Updates the task with the specified ID if it exists</summary>
        /// <param name="id">Task ID</param>
        /// <param name="updatedTask">Updated task</param>
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Models.Task updatedTask)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = this.dbContext.Tasks.SingleOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            task.Update(updatedTask);

            this.dbContext.Update(task);
            this.dbContext.SaveChanges();

            return Ok();
        }

        /// <summary>Deletes the task with the specified ID if it exists</summary>
        /// <param name="id">Task ID</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = this.dbContext.Tasks.SingleOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            this.dbContext.Tasks.Remove(task);
            this.dbContext.SaveChanges();

            return Ok();
        }
    }
}
