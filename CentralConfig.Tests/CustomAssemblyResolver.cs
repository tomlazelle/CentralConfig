using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;
using CentralConfig.Controllers;

namespace CentralConfig.Tests
{
    public class TestWebApiResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly>
            {
                typeof (ConfigController).Assembly
            };
        }
    }
}