using CentralConfig.Client;
using NUnit.Framework;

namespace CentralConfig.Tests
{
    [TestFixture]
    public class GetABasicCongTestFixture : InMemoryApiServer
    {
        private ConfigPortal portal;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            portal = new ConfigPortal(Server.HttpClient);

            portal.RemoveAll();
        }


        [Test]
        public void A_add_1_config_item()
        {
            var expected = "I Dit It";

            portal.Add("DoIt", expected, "", "dev");

            var result = portal.GetConfig<MyCustomConfig>("dev");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DoIt, Is.EqualTo(expected));
        }

        [Test]
        public void B_add_the_same_config_item_with_a_different_value()
        {
            var expected = "I Dit It Again";

            portal.Add("DoIt", expected, "", "dev");

            var result = portal.GetConfig<MyCustomConfig>("dev");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DoIt, Is.EqualTo(expected));
        }
    }

    public class MyCustomConfig
    {
        public string DoIt { get; set; }
        public string DoItToIt { get; set; }
    }
}