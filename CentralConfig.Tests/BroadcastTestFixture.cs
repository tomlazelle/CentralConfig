using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CentralConfig.Client;
using CentralConfig.Models;
using Microsoft.Owin.Hosting;
using Owin;
using Raven.Client;
using Should;

namespace CentralConfig.Tests
{


    public class BroadcastTests : InMemoryApiServer
    {
        private ConfigPortal portal;


        public BroadcastTests()
        {

            portal = new ConfigPortal(Server.HttpClient);

            //            portal.RemoveAll();
        }

        //        private void ClearDocuments()
        //        {
        //            using (var session = InMemoryApiServer.Container.GetInstance<IDocumentStore>().OpenSession())
        //            {
        //               var broadCastModels = session.Query<BroadCastNotifyModel>().ToList();
        //                foreach (var model in broadCastModels)
        //                {
        //                    session.Delete(model);
        //                }
        //
        //
        //                var nameValues = session.Query<NameValueModel>().ToList();
        //                foreach (var model in nameValues)
        //                {
        //                    session.Delete(model);
        //                }
        //
        //                session.SaveChanges();
        //            }
        //        }

        
        public void A_add_broadcast_event_notify()
        {

            portal.AddWatch("DoIt", "http://localhost:9000/WebHook");

            var watchers = portal.GetWatchers();

            watchers.ShouldNotBeNull();
            watchers.Count().ShouldBeGreaterThan(0);

        }

        
        public void B_add_config_item_change_and_wait_for_callback()
        {
            IsCompleted = false;

            using (var listener = new WebHookListener())
            {
                var expected = "I Dit It";

                portal.Add("DoIt", expected, "", "dev");

                portal.Add("DoIt", "newValue", "", "dev");
            }

            while (!IsCompleted)
            {

            }
            
            IsCompleted.ShouldBeTrue();
            
        }

        void BroadcastTestFixture_EventReceived(object sender, EventArgs e)
        {
            IsCompleted = true;
        }

        public static bool IsCompleted;

    }


    //    public class HookListener
    //    {
    //        private static bool isDone;
    //        public static void Completed()
    //        {
    //            
    //        }
    //
    //        public async Task<bool> Waiting()
    //        {
    //            return isDone;
    //        }
    //    }

    public class WebHookController : ApiController
    {
        public HttpResponseMessage Post(OnChangedMessage message)
        {
            BroadcastTests.IsCompleted = true;

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }

    public class WebHookListener : IDisposable
    {
        string baseAddress = "http://*:9000/";
        IDisposable webApp = null;
        public WebHookListener()
        {
            var options = new StartOptions(baseAddress)
            {
                ServerFactory = "Microsoft.Owin.Host.HttpListener",

            };

            options.Urls.Add("http://api.localhost.com");

            webApp = WebApp.Start<WebHookStartup>(new StartOptions(baseAddress));

        }

        public void Dispose()
        {
            webApp.Dispose();
        }
    }

    public class WebHookStartup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(config);
        }
    }
}