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
            FileVersionInfo info  = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            About           model = new About();

            model.AppName   = info.ProductName;
            model.Copyright = info.LegalCopyright;
            model.Url       = "https://simple-web-app-mvc-dotnet.azurewebsites.net/";
            model.Version   = ("Version " + info.ProductVersion);

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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET [ /, /Home/, /Home/Index ]
        public IActionResult Index()
        {
            ViewData["message_short"] = "Welcome to my simple web app";
            ViewData["message_long"]  = "This simple web app is made using ASP.NET Core 2.0 MVC and hosted on Azure Cloud Services.";

            return View();
        }
    }
}
