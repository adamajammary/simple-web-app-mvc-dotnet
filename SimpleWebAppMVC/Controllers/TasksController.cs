using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebAppMVC.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Controllers
{
    // ValidateAntiForgeryToken: http://go.microsoft.com/fwlink/?LinkId=317598

    public class TasksController(AppDbContext dbCtx) : Controller
    {
        private readonly AppDbContext dbContext = dbCtx;

        // GET /Tasks/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Models.Task());
        }

        // POST /Tasks/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Date,Status")] Models.Task newTask)
        {
            if (ModelState.IsValid)
            {
                var task = new Models.TaskDbModel(newTask);

                this.dbContext.Add(task);

                await this.dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(newTask);
        }

        // GET /Tasks/Details/<id>
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == id);

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

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST /Tasks/Delete/<id>
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == id);

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

            var task = await this.dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST /Tasks/Edit/<id>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description,Date,Status")] Models.TaskDbModel task)
        {
            if (id != task.Id)
                return NotFound();

            if (!this.dbContext.Tasks.Any(t => t.Id == id))
                return NotFound();

            if (ModelState.IsValid)
            {
                this.dbContext.Update(task);

                await this.dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }
        
        // GET [ /Tasks/, /Tasks/Index ]
        [HttpGet]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.TitleSort       = (sort == "title"       ? "title_desc"       : "title");
            ViewBag.DescriptionSort = (sort == "description" ? "description_desc" : "description");
            ViewBag.DateSort        = (sort == "date"        ? "date_desc"        : "date");
            ViewBag.StatusSort      = (sort == "status"      ? "status_desc"      : "status");

            ViewBag.Sort = sort;

            return View(await this.GetSorted(sort).ToListAsync());
        }

        // GET /Tasks/GetJSON/<sort>
        [HttpGet]
        public async Task<IActionResult> GetJSON(string sort)
        {
            return Json(await this.GetSorted(sort).ToListAsync());
        }

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
                _                  => tasks.OrderBy(s => s.Title),
            };

            return tasks;
        }
    }
}
