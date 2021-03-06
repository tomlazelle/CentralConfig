// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Linq;
using CentralConfig.Models;
using Microsoft.Data.OData.Query.SemanticAst;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace CentralConfig.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                x.For<IDocumentStore>().LifecycleIs(Lifecycles.Singleton).Use(s => SetupDocumentStore());
            });

            return container;
        }

        private static IDocumentStore SetupDocumentStore()
        {
            var store = new EmbeddableDocumentStore
            {
//                                RunInMemory = true
                Url = "http://localhost:8080",
                DefaultDatabase = "CentralConfig"
            };

            store.Initialize();
            IndexCreation.CreateIndexes(typeof(ConfigItemsIndex).Assembly, store);

            return store;
        }
    }

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
                                Name =item.Name,
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