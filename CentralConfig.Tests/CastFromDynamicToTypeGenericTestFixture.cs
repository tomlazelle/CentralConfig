using System.Dynamic;
using NUnit.Framework;
using CentralConfig.Common;

namespace CentralConfig.Tests
{
    [TestFixture]
    public class CastFromDynamicToTypeGenericTestFixture
    {


        [Test]
        public void TestName()
        {
            dynamic customer = new ExpandoObject();
            customer.Name = "Skippy";

            Customer actual = ObjectExtensions.ToPOCO(customer, new Customer());

            Assert.That(actual.Name, Is.EqualTo(customer.Name));
        }
    }

    public class Customer
    {
        public string Name { get; set; }
    }


}