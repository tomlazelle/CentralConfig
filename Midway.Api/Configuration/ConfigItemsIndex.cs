using System.Linq;
using Midway.Api.Models;
using Raven.Client.Documents.Indexes;

namespace Midway.Api.Configuration
{
    public class ConfigItemsIndex : AbstractIndexCreationTask<NameValueModel, ConfigItemsIndex.Result>
    {
        public class Result
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string GroupName { get; set; }
            public string Environment { get; set; }
            public int Version { get; set; }
        }

        public ConfigItemsIndex()
        {
            Map = nameValues => from item in nameValues
                                select new Result
                                {
                                    Name = item.Name,
                                    Environment = item.Environment,
                                    GroupName = item.GroupName,
                                    Value = item.Value,
                                    Version = item.Version
                                };
            //            
            //
            //            Reduce = items => from x in items
            //                              group x by new { x.Name, x.GroupName, x.Environment,x.Version} into g
            //                              select new Result
            //                              {
            //                                  Name = g.Key.Name,
            //                                  GroupName = g.Key.GroupName,
            //                                  Value = g.FirstOrDefault(x => x.Version == g.Max(m => m.Version) && x.Environment == g.Key.Environment && x.Name == g.Key.Name && x.GroupName == g.Key.GroupName).Value,
            //                                  Environment = g.Key.Environment,
            //                                  Version = g.Max(x=>x.Version)
            //                              };


            Reduce = data => from c in data
                             group c by new { c.Name, c.Environment, c.GroupName } into x
                             let z = x.Single(s => s.Version == x.Max(m => m.Version))
                             select new Result
                             {
                                 Name = z.Name,
                                 Environment = z.Environment,
                                 GroupName = z.GroupName,
                                 Value = z.Value,
                                 Version = z.Version,
                             };

            StoreAllFields(FieldStorage.Yes);
        }
    }
}