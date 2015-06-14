using System.Linq;
using CentralConfig.Client;
using Should;

namespace CentralConfig.Tests
{
    public class GetABasicCongTests : InMemoryApiServer
    {
        private ConfigPortal portal;

        public GetABasicCongTests()
        {
            portal = new ConfigPortal(Server.HttpClient);

            portal.RemoveAll();
        }

        public void A_add_1_config_item()
        {
            var expected = "I Dit It";

            portal.Add("DoIt", expected, "", "dev");

            var result = portal.GetConfig<MyCustomConfig>("dev");
            result.ShouldNotBeNull();
            result.DoIt.ShouldEqual(expected);
        }

        public void B_add_the_same_config_item_with_a_different_value()
        {
            var expected = "I Dit It Again";

            portal.Add("DoIt", expected, "", "dev");

            var result = portal.GetConfig<MyCustomConfig>("dev");

            result.ShouldNotBeNull();
            result.DoIt.ShouldEqual(expected);
        }
    }


    public class can_use_automapper_profile_wth_get_config : InMemoryApiServer
    {
        private ConfigPortal portal;

        public void Setup()
        {
            portal = new ConfigPortal(Server.HttpClient);

            portal.RemoveAll();
        }

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


            result.ShouldNotBeNull();

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