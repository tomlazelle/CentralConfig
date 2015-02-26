using System.Linq;
using System.Web.Mvc;

namespace WebClientTestApp.Controllers
{
    public class HomeController : Controller
    {
        private string partialData = string.Empty;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var settings = WebApiApplication.ConfigSettings.GetConfig<MyCustomConfig>("dev", items =>
            {
                return new MyCustomConfig
                {
                    G1 = items.Where(x=>x.GroupName == "g1").ToDictionary(k=>k.Name,e=>e.Value),
                    TestValue2 = items.FirstOrDefault(x=>x.Name == "TestValue2").Value
                };
            });

            ViewBag.V1 = settings.G1["TestValue1"] + partialData;
            ViewBag.V2 = settings.G1["ConnectionString"];
            ViewBag.V3 = settings.TestValue2;

            return View();
        }

        public ActionResult ChangeValue(string id)
        {

            WebApiApplication.ConfigSettings.Add("TestValue1", id, "g1", "dev");
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