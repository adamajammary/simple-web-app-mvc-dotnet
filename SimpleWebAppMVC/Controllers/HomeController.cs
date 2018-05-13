using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleWebAppMVC.Models;

namespace SimpleWebAppMVC.Controllers
{
    /**
     * Home Controller
     */
    public class HomeController : Controller
    {
        /**
         * GET: /Home/About
         */
        public IActionResult About()
        {
            AboutModel model = new AboutModel();

            model.AppName   = "Simple Web App MVC";
            model.Copyright = "2018 Adam A. Jammary";
            model.Url       = "http://simple-web-app-mvc-dotnet.azurewebsites.net/";
            model.Version   = "Version 1.0.0";

            return View(model);
        }

        /**
         * GET: /Home/Error
         */
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /**
         * GET: [ /, /Home/, /Home/Index ]
         */
        public IActionResult Index()
        {
            ViewData["message_short"] = "Welcome to my simple web app";
            ViewData["message_long"]  = "This simple web app was made using ASP.NET Core 2.0 MVC and hosted on Azure Cloud Services.";

            return View();
        }
    }
}
