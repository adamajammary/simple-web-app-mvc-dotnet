using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebAppMVC.Data;

namespace SimpleWebAppMVC.Controllers
{
    [Produces("application/json")]
    [Route("api/Tasks")]
    public class TasksApiController : Controller
    {
        private readonly AppDbContext dbContext;

        /**
         * TasksApiController constructor.
         * @param dbCtx Application database context
         */
        public TasksApiController(AppDbContext dbCtx)
        {
            this.dbContext = dbCtx;
        }

        // GET api/Tasks
        [HttpGet]
        public JsonResult Get()
        {
            return Json(from task in this.dbContext.Tasks select task);
        }

        // GET api/Tasks/<id>
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

        // POST api/Tasks
        [HttpPost]
        public IActionResult Post([FromBody] Models.Task taskModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            this.dbContext.Add(taskModel);
            this.dbContext.SaveChanges();

            return CreatedAtRoute("GetTask", new { id = taskModel.Id }, taskModel);
        }

        // PUT api/Tasks/<id>
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Models.Task taskModel)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = this.dbContext.Tasks.SingleOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            task.Update(taskModel);
            this.dbContext.Update(task);
            this.dbContext.SaveChanges();

            return Ok();
        }

        // DELETE api/Tasks/<id>
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
