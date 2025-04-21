using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebAppMVC.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Controllers
{
    // ValidateAntiForgeryToken: http://go.microsoft.com/fwlink/?LinkId=317598

    [Authorize]
    public class TasksController(AppDbContext dbCtx) : Controller
    {
        private readonly AppDbContext dbContext = dbCtx;

        /// <param name="sort">Sort column and order</param>
        /// <returns>a list of tasks sorted by the specified sort column and order</returns>
        private IQueryable<Models.TaskDbModel> GetSorted(string sort)
        {
            var tasks = from task in this.dbContext.Tasks select task;

            tasks = sort switch
            {
                "title"            => tasks.OrderBy(s => s.Title),
                "title_desc"       => tasks.OrderByDescending(s => s.Title),
                "description"      => tasks.OrderBy(s => s.Description),
                "description_desc" => tasks.OrderByDescending(s => s.Description),
                "date"             => tasks.OrderBy(s => s.Date),
                "date_desc"        => tasks.OrderByDescending(s => s.Date),
                "status"           => tasks.OrderBy(s => s.Status),
                "status_desc"      => tasks.OrderByDescending(s => s.Status),
                "createdby"        => tasks.OrderBy(s => s.CreatedBy),
                "createdby_desc"   => tasks.OrderByDescending(s => s.CreatedBy),
                _                  => tasks.OrderBy(s => s.Title),
            };

            return tasks;
        }

        // GET /Tasks/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Models.TaskModel());
        }

        // POST /Tasks/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Date,Status")] Models.TaskModel newTask)
        {
            if (!ModelState.IsValid)
                return View(newTask);

            var task = new Models.TaskDbModel(newTask, this.User.Identity.Name);

            await this.dbContext.AddAsync(task);
            await this.dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET /Tasks/Details/<id>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // GET /Tasks/Delete/<id>
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (task.CreatedBy != this.User.Identity.Name)
                return Forbid();

            return View(task);
        }

        // POST /Tasks/Delete/<id>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, [Bind("Id,CreatedBy")] Models.TaskDbModel task)
        {
            if (id != task.Id)
                return BadRequest();

            if (task.CreatedBy != this.User.Identity.Name)
                return Forbid();

            var taskExists = await this.dbContext.Tasks.AnyAsync(t => (t.Id == task.Id) && (t.CreatedBy == task.CreatedBy));

            if (!taskExists)
                return NotFound();

            this.dbContext.Tasks.Remove(task);

            await this.dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET /Tasks/Edit/<id>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (task.CreatedBy != this.User.Identity.Name)
                return Forbid();

            return View(task);
        }

        // POST /Tasks/Edit/<id>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,CreatedBy,Title,Description,Date,Status")] Models.TaskDbModel task)
        {
            if (id != task.Id)
                return BadRequest();

            if (task.CreatedBy != this.User.Identity.Name)
                return Forbid();

            var taskExists = await this.dbContext.Tasks.AnyAsync(t => (t.Id == task.Id) && (t.CreatedBy == task.CreatedBy));

            if (!taskExists)
                return NotFound();

            if (!ModelState.IsValid)
                return View(task);

            this.dbContext.Update(task);

            await this.dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET [ /Tasks/, /Tasks/Index ]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.TitleSort       = (sort == "title"       ? "title_desc"       : "title");
            ViewBag.DescriptionSort = (sort == "description" ? "description_desc" : "description");
            ViewBag.DateSort        = (sort == "date"        ? "date_desc"        : "date");
            ViewBag.StatusSort      = (sort == "status"      ? "status_desc"      : "status");
            ViewBag.CreatedBySort   = (sort == "createdby"   ? "createdby_desc"   : "createdby");

            ViewBag.Sort = sort;

            return View(await this.GetSorted(sort).ToListAsync());
        }

        // GET /Tasks/GetJSON/<sort>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetJSON(string sort)
        {
            return Json(await this.GetSorted(sort).ToListAsync());
        }
    }
}
