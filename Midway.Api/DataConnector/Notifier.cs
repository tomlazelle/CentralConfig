using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Midway.Api.Models;
using Newtonsoft.Json;
using Raven.Client.Documents;

namespace Midway.Api.DataConnector
{
    public class Notifier : INotifier
    {
        private readonly IDocumentStore _documentStore;
        public Notifier(IDocumentStore documentStore)
        {
            _documentStore = documentStore;

        }
        public void OnChange(string name)
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
                    var stringData = JsonConvert.SerializeObject(new OnChangedMessage { Changed = true, Name = broadcast.Name });
                    var content = new StringContent(stringData, Encoding.UTF8, "application/json");
                    var result = client.PostAsync(new Uri(broadcast.UrlCallback), content).Result;

                    var actual = result.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }
}