using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using CentralConfig.DependencyResolution;
using Microsoft.Owin.Testing;
using Owin;
using StructureMap;

namespace CentralConfig.Tests
{
    public abstract class InMemoryApiServer : IDisposable
    {
        public static IContainer Container;
        public string BaseUrl = "http://localhost/";
        public TestServer Server;

        protected InMemoryApiServer()
        {
            Container = IoC.Initialize();

            Server = TestServer.Create<MyStartup>();
        }

        public void Dispose()
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