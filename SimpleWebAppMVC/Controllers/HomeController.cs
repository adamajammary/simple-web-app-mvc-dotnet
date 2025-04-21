using Microsoft.AspNetCore.Mvc;
using SimpleWebAppMVC.Models;
using System.Diagnostics;
using System.Reflection;

namespace SimpleWebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET /Home/About
        [HttpGet]
        public IActionResult About()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var info     = FileVersionInfo.GetVersionInfo(location);

            var model = new AboutModel
            {
                AppName   = info.ProductName,
                Copyright = info.LegalCopyright,
                Url       = "https://www.jammary.com/",
                Version   = string.Format("Version {0}.{1}.{2}", info.ProductMajorPart, info.ProductMinorPart, info.ProductBuildPart)
            };

            return View(model);
        }

        // GET /Home/Error
        [HttpGet]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = (Activity.Current?.Id ?? HttpContext.TraceIdentifier)
            };

            return View(model);
        }

        // GET [ /, /Home/, /Home/Index ]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Header      = "Welcome to my simple web app";
            ViewBag.Description = "This simple web app is made using ASP.NET 9.0 MVC.";

            return View();
        }
    }
}
