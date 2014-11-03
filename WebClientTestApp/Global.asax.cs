using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CentralConfig.Client;

namespace WebClientTestApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static ConfigPortal ConfigSettings;

        protected void Application_Start()
        {

            ConfigSettings = new ConfigPortal("http://localhost:59119/");
            ConfigSettings.Add("TestValue1", "I am one", "", "dev");
            ConfigSettings.Add("ConnectionString", "I am connected", "", "dev");
            ConfigSettings.Add("TestValue2", "I am 2", "", "dev");


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
