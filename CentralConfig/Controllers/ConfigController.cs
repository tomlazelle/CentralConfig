using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using CentralConfig.Models;
using Raven.Client;

namespace CentralConfig.Controllers
{
    public class ConfigController : ApiController
    {
        private readonly IDocumentStore _documentStore;

        public ConfigController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public HttpResponseMessage Delete()
        {
            using (var session = _documentStore.OpenSession())
            {
                var allResults = session.Query<NameValueModel>().Take(1024).ToList();

                foreach (var item in allResults)
                {
                    session.Delete(item);
                }

                session.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Post(NameValueRequest request)
        {
            var newModel = new NameValueModel
            {
                Name = request.Name,
                Value = request.Value,
                GroupName = request.GroupName,
                Environment = request.Environment,
                Version = 1
            };

            using (var session = _documentStore.OpenSession())
            {
                var model = session
                    .Query<NameValueModel>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Where(x => x.Name == request.Name && x.GroupName == request.GroupName)
                    .FirstOrDefault();


                if (model != null)
                {
                    newModel.Version = model.Version + 1;
                }


                session.Store(newModel);

                session.SaveChanges();
            }
            NotifyOnChange(newModel.Name);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        private void NotifyOnChange(string name)
        {
            List<BroadCastNotifyModel> broadcasts;
            using (var session = _documentStore.OpenSession())
            {
                broadcasts = session.Query<BroadCastNotifyModel>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Where(x => x.Name == name && x.EventName == "OnChanged")
                    .ToList();
            }

            var client = new HttpClient();
            foreach (var broadcast in broadcasts)
            {
                try
                {
                    var result = client.PostAsync(new Uri(broadcast.UrlCallback), new OnChangedMessage { Changed = true, Name = broadcast.Name }, new JsonMediaTypeFormatter()).Result;

                    var actual = result.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public IEnumerable<NameValueModel> Get(string environment)
        {
            using (var session = _documentStore.OpenSession())
            {
                
                    return session.Query<NameValueModel>()
                        .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                        .Where(x =>  x.Environment == environment).ToList();
                

            }
        }

        public IList<NameValueRequest> Get()
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
    }
}