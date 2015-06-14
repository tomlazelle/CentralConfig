using System.Dynamic;
using CentralConfig.Common;
using Should;

namespace CentralConfig.Tests
{
    public class CastFromDynamicToTypeGenerics
    {
        public void TestName()
        {
            dynamic customer = new ExpandoObject();
            customer.Name = "Skippy";

            Customer actual = ObjectExtensions.ToPOCO(customer, new Customer());

            string expected = customer.Name;
            string actualName = actual.Name;
            
            actualName.ShouldEqual(expected);
        }
    }

    public class Customer
    {
        public string Name { get; set; }
    }
}