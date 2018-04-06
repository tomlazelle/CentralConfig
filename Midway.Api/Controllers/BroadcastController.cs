using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Midway.Api.Models;
using Raven.Client.Documents;

namespace Midway.Api.Controllers
{
    public class BroadcastController : Controller
    {
        private readonly IDocumentStore _documentStore;

        public BroadcastController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        [HttpPost]
        [Route("/BroadCast")]
        public async Task<ActionResult> Post([FromBody]BroadCastNotifyRequest message)
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


            return Created(new Uri("/"),"");
        }

        [HttpGet]
        [Route("/BroadCast")]
        public async Task<ActionResult> Get()
        {
            List<BroadCastNotifyRequest> result;

            using (var session = _documentStore.OpenSession())
            {
                result = session.Query<BroadCastNotifyModel>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .Select(x => new BroadCastNotifyRequest
                    {
                        Name = x.Name,
                        EventName = x.EventName,
                        GroupName = x.GroupName,
                        UrlCallback = x.UrlCallback
                    }).ToList();
            }

            return Ok(result);
        }
    }
}
