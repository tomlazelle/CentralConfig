using System.Web.Mvc;

namespace WebClientTestApp.Controllers
{
    public class HomeController : Controller
    {
        private string partialData = string.Empty;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var settings = WebApiApplication.ConfigSettings.GetConfig<MyCustomConfig>("dev");

            ViewBag.V1 = settings.TestValue1 + partialData;
            ViewBag.V2 = settings.ConnectionString;
            ViewBag.V3 = settings.TestValue2;

            return View();
        }

        public ActionResult ChangeValue(string id)
        {

            WebApiApplication.ConfigSettings.Add("TestValue1", id, "", "dev");
            return View("Index");
        }

        [HttpPost]
        public ActionResult ChangeDetected(OnChangedMessage model)
        {
            if (model.Changed)
            {
                partialData = "data changed";
            }
            return View("Index");
        }
    }

    public class OnChangedMessage
    {
        public bool Changed { get; set; }
        public string Name { get; set; }
    }
}