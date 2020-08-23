using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleWebAppMVC.Models;
using System.Reflection;

namespace SimpleWebAppMVC.Controllers
{
    /**
     * Home Controller
     */
    public class HomeController : Controller
    {
        // GET /Home/About
        public IActionResult About()
        {
            string          location    = Assembly.GetExecutingAssembly().Location;
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(location);

            About model = new About
            {
                AppName   = versionInfo.ProductName,
                Copyright = versionInfo.LegalCopyright,
                Url       = "https://www.jammary.com/",
                Version   = ("Version " + versionInfo.ProductVersion)
            };

            return View(model);
        }

        // GET /Home/API
        [HttpGet]
        public IActionResult API()
        {
            ViewData["message_short"] = "Tasks API";

            return View();
        }

        // GET /Home/Error
        public IActionResult Error()
        {
            ErrorViewModel model = new ErrorViewModel
            {
                RequestId = (Activity.Current?.Id ?? HttpContext.TraceIdentifier)
            };

            return View(model);
        }

        // GET [ /, /Home/, /Home/Index ]
        public IActionResult Index()
        {
            ViewData["message_short"] = "Welcome to my simple web app";
            ViewData["message_long"]  = "This simple web app is made using ASP.NET Core 3.1 MVC.";

            return View();
        }
    }
}
