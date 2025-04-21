using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using SimpleWebAppMVC.Data;
using SimpleWebAppMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Controllers
{
    [ApiController]
    [OpenApiTag("Tasks")]
    [Produces("application/json")]
    [Route("api/tasks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TasksApiController(AppDbContext dbCtx) : Controller
    {
        private readonly AppDbContext dbContext = dbCtx;

        /// <summary>Returns a list of all the tasks</summary>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Get()
        {
            var tasks = from task in this.dbContext.Tasks select task;

            return Json(tasks);
        }

        /// <summary>Returns the task with the specified ID if it exists</summary>
        /// <param name="id">Task ID</param>
        [AllowAnonymous]
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return Json(task);
        }

        /// <summary>Creates a new task</summary>
        /// <param name="newTask">New task</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskModel newTask)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var task = new TaskDbModel(newTask, this.User.Identity.Name);

            await this.dbContext.AddAsync(task);
            await this.dbContext.SaveChangesAsync();

            return CreatedAtRoute("Get", new { id = task.Id }, task);
        }

        /// <summary>Updates the task with the specified ID if it exists</summary>
        /// <param name="id">Task ID</param>
        /// <param name="updatedTask">Updated task</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] TaskModel updatedTask)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = this.dbContext.Tasks.SingleOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (task.CreatedBy != this.User.Identity.Name)
                return Forbid(JwtBearerDefaults.AuthenticationScheme);

            task.Update(updatedTask);

            this.dbContext.Update(task);

            await this.dbContext.SaveChangesAsync();

            return Ok(task);
        }

        /// <summary>Deletes the task with the specified ID if it exists</summary>
        /// <param name="id">Task ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = this.dbContext.Tasks.SingleOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (task.CreatedBy != this.User.Identity.Name)
                return Forbid(JwtBearerDefaults.AuthenticationScheme);

            this.dbContext.Tasks.Remove(task);

            await this.dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
