using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CentralConfig.Models;
using Raven.Client;

namespace CentralConfig.Controllers
{
    public class BroadcastController : ApiController
    {
        private readonly IDocumentStore _documentStore;

        public BroadcastController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public HttpResponseMessage Post(BroadCastNotifyRequest message)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(new BroadCastNotifyModel
                {
                    EventName = message.EventName,
                    Name = message.Name,
                    GroupName = message.GroupName,
                    UrlCallback = message.UrlCallback
                });

                session.SaveChanges();
            }


            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public IEnumerable<BroadCastNotifyRequest> Get()
        {
            List<BroadCastNotifyRequest> result;

            using (var session = _documentStore.OpenSession())
            {
                result = session.Query<BroadCastNotifyModel>()
                    .Customize(x=>x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Select(x => new BroadCastNotifyRequest
                    {
                        Name = x.Name,
                        EventName = x.EventName,
                        GroupName = x.GroupName,
                        UrlCallback = x.UrlCallback
                    }).ToList();
            }

            return result;
        }
    }
}