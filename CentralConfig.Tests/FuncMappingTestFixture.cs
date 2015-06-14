using System;
using System.Collections.Generic;
using System.Linq;

using Should;

namespace CentralConfig.Tests
{
    
    public class FuncMappingTests
    {
       


        
        public void can_use_a_mapping_action()
        {
            var testItems = new[]
            {
                new MapItem
                {
                    Name = "NG1",
                    Group = "G1",
                    Value = "VG1"
                },
                  new MapItem
                {
                    Name = "NG2",
                    Group = "G1",
                    Value = "VG2"
                },
                  new MapItem
                {
                    Name = "N1",
                    Value = "V1"
                }
            };


            var func = new Func<IEnumerable<MapItem>, MapToMe>(items => new MapToMe
            {
                G1 = items.Where(x => x.Group == "G1").ToDictionary(x => x.Name, z => z.Value),
                Settings = items.Where(x => x.Group != "G1").ToDictionary(x => x.Name, z => z.Value)
            });

            var result = func.Invoke(testItems);
                
//                new MapToMe
//            {
//                G1 = testItems.Where(x => x.Group == "G1").ToDictionary(x => x.Name, z => z.Value), 
//                Settings = testItems.Where(x => x.Group != "G1").ToDictionary(x => x.Name, z => z.Value)
//            };


            result.G1.Count.ShouldEqual(2);
            result.Settings.Count.ShouldEqual(1);

        }

    }



    public class MapItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }



    public class MapToMe
    {
        public IDictionary<string, string> G1 { get; set; }
        public IDictionary<string, string> Settings { get; set; }

    }
}