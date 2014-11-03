using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClientTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var settings = WebApiApplication.ConfigSettings.GetConfig<MyCustomConfig>("dev");

            ViewBag.V1 = settings.TestValue1;
            ViewBag.V2 = settings.ConnectionString;
            ViewBag.V3 = settings.TestValue2;

            return View();
        }
    }
}
