using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fixie;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace CentralConfig.Tests.Common
{
    public class BasicAutoFixtureParam : ParameterSource
    {
        public IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            foreach (var parameterInfo in method.GetParameters())
            {
                var paramType = parameterInfo.ParameterType;
                var value = CreateAutoFixtureObjectFromType(paramType);

                yield return new[] { value };
            }
        }

        private object CreateAutoFixtureObjectFromType(Type typeToCreate)
        {
            if (typeToCreate.IsInterface)
            {
                var foundType = typeToCreate.Assembly.GetTypes().FirstOrDefault(p => typeToCreate.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

                var fixture = new Ploeh.AutoFixture.Fixture();
                fixture.Register(() => fixture.Create(foundType));


                return new SpecimenContext(fixture).Resolve(foundType);
            }

            return new SpecimenContext(new Ploeh.AutoFixture.Fixture()).Resolve(typeToCreate);
        }
    }
}