using System.Collections.Generic;
using Midway.Api.Configuration;
using Midway.Api.Models;
using Raven.Client.Documents;
using System;
using System.Linq;

namespace Midway.Api.DataConnector
{
    public class RavenDbDataProvider : IDataProvider
    {
        private IDocumentStore _documentStore;
        private readonly INotifier _notifier;

        public RavenDbDataProvider(
            IDocumentStore documentStore,
            INotifier notifier)
        {
            _documentStore = documentStore;
            _notifier = notifier;
        }
        public bool Create(NameValueModel nameValueModel)
        {           

            using (var session = _documentStore.OpenSession())
            {
                var model = session
                    .Query<ConfigItemsIndex.Result, ConfigItemsIndex>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Where(x => 
                        x.Name == nameValueModel.Name 
                        && x.GroupName == nameValueModel.GroupName 
                        && x.Environment == nameValueModel.Environment)
                    .ProjectInto<ConfigItemsIndex.Result>()
                    .OrderByDescending(x => x.Version)
                    .FirstOrDefault();


                if (model != null)
                {
                    nameValueModel.Version = model.Version + 1;
                }


                session.Store(nameValueModel);

                session.SaveChanges();
            }
            
            _notifier.OnChange(nameValueModel.Name);

            return true;
        }

        public IEnumerable<NameValueRequest> GetAll()
        {
             IList<NameValueRequest> result;
            using (var session = _documentStore.OpenSession())
            {
                var r1 = session.Query<NameValueModel>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Take(1024).ToList()
                    .GroupBy(x => x.Name)
                    .Select(g => g.OrderByDescending(p => p.Version).First());


                result = r1.Select(x => new NameValueRequest
                {
                    Name = x.Name,
                    GroupName = x.GroupName,
                    Value = x.Value,
                    Environment = x.Environment
                }).ToList();
            }

            return result;
        }

        public IEnumerable<NameValueRequest> GetEnvironment(string environment)
        {
             IEnumerable<NameValueRequest> result;

            using (var session = _documentStore.OpenSession())
            {

                var temp = session.Query<ConfigItemsIndex.Result, ConfigItemsIndex>()
                        .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                        .Where(x => x.Environment == environment)
                        .ProjectInto<ConfigItemsIndex.Result>().ToList();

                result = temp.Select(x => new NameValueRequest
                {
                    Name = x.Name,
                    Environment = x.Environment,
                    GroupName = x.GroupName,
                    Value = x.Value
                });


            }
            return result;
        }
    }
}