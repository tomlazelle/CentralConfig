using System;
using System.Linq;
using Fixie;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace CentralConfig.Tests.Common
{
    public class TestcaseClassPerClassConvention : Convention
    {
        public TestcaseClassPerClassConvention()
        {
            Classes.NameEndsWith("Tests");
            //.Where(t => t.GetConstructors().All(ci => ci.GetParameters().Length == 0));

            Methods.Where(mi => mi.IsPublic && mi.IsVoid());

//            Parameters.Add<BasicAutoFixtureParam>();


            ClassExecution
                .CreateInstancePerClass();
                
        }

//        private object CreateFromFixture(Type type)
//        {
//            var foundType = type.Assembly.GetTypes().FirstOrDefault(p => type.IsAssignableFrom(p));
//
//            var fixture = new Ploeh.AutoFixture.Fixture();
//            fixture.Register(() => fixture.Create(foundType));
//
//
//            return new SpecimenContext(fixture).Resolve(type);
//        }
    }
}