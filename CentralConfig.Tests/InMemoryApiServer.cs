using System.Web.Http;
using System.Web.Http.Dispatcher;
using CentralConfig.DependencyResolution;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;
using StructureMap;

namespace CentralConfig.Tests
{
    public abstract class InMemoryApiServer
    {
        public static IContainer Container;
        public string BaseUrl = "http://localhost/";
        public TestServer Server;

        protected InMemoryApiServer()
        {
            Container = IoC.Initialize();

            Server = TestServer.Create<MyStartup>();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Server.Dispose();
            Container.Dispose();
        }


        internal class MyStartup
        {
            public void Configuration(IAppBuilder app)
            {
                var config = new HttpConfiguration();
                config.Services.Replace(typeof (IAssembliesResolver), new TestWebApiResolver());
                config.DependencyResolver = new StructureMapDependencyResolver(Container);
                WebApiConfig.Register(config);
                app.UseWebApi(config);
            }
        }
    }
}