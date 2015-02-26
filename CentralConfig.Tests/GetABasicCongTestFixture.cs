using System.Collections.Generic;
using System.Linq;
using CentralConfig.Client;
using CentralConfig.Models;
using NUnit.Framework;
using Should;

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

    [TestFixture]
    public class can_use_automapper_profile_wth_get_config : InMemoryApiServer
    {
        private ConfigPortal portal;

        [SetUp]
        public void Setup()
        {
            portal = new ConfigPortal(Server.HttpClient);

            portal.RemoveAll();
        }

        [Test]
        public void cann_get_a_confg_with_automapper()
        {
            portal.Add("NG1", "VG1", "G1", "dev");
            portal.Add("NG2", "VG2", "G1", "dev");

            portal.Add("NG1", "VG1", "G1", "prod");
            portal.Add("NG2", "VG2", "G1", "prod");

            portal.Add("N1", "V1", "", "dev");

            var result = portal.GetConfig("dev", items => new MapToMe
            {
                G1 = items.Where(x => x.GroupName == "G1").ToDictionary(x => x.Name, z => z.Value),
                Settings = items.Where(x => x.GroupName != "G1").ToDictionary(x => x.Name, z => z.Value)
            });

            Assert.That(result, Is.Not.Null);
            result.G1.Count.ShouldEqual(2);
            result.Settings.Count.ShouldEqual(1);
        }
    }


    public class MyCustomConfig
    {
        public string DoIt { get; set; }
        public string DoItToIt { get; set; }
    }
}