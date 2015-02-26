using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using CentralConfig.Models;

namespace CentralConfig.Client
{
    public class ConfigPortal
    {
        private HttpClient _client;
        private string _baseUrl = "http://localhost/";

        public ConfigPortal(string configPortalUrl)
        {
            _baseUrl = configPortalUrl;
            _client = new HttpClient();
        }

        public ConfigPortal(HttpClient client)
        {
            _client = client;
        }

        public T GetConfig<T>(string environment) where T : new()
        {
            var builder = new StringBuilder(_baseUrl);
            builder.Append("api/config");
            builder.AppendFormat("?environment={0}", environment);

            var message = HttpMessageRequest.CreateRequest(builder.ToString(), HttpMethod.Get);
            var result = _client.SendAsync(message).Result;

            var data = result.Content.ReadAsAsync<IEnumerable<NameValueRequest>>().Result.ToList();

            var dataResult = new T();

            foreach (var item in data)
            {
                dataResult.GetType().GetProperty(item.Name).SetValue(dataResult, item.Value);
            }


            return dataResult;
        }

        public T GetConfig<T>(string environment, Func<IEnumerable<NameValueRequest>,T> mappingFunction) where T : new()
        {
            var builder = new StringBuilder(_baseUrl);
            builder.Append("api/config");
            builder.AppendFormat("?environment={0}", environment);

            var message = HttpMessageRequest.CreateRequest(builder.ToString(), HttpMethod.Get);
            var result = _client.SendAsync(message).Result;

            
            
            
            var data = result.Content.ReadAsAsync<IEnumerable<NameValueRequest>>().Result.ToList();

            var dataResult = mappingFunction.Invoke(data);

            return dataResult;
        }

        public void Add(string name, string value, string groupName,string environment)
        {
            var message = HttpMessageRequest.CreateRequest(_baseUrl + "api/config", HttpMethod.Post, new NameValueRequest
            {
                Name = name,
                Value = value,
                GroupName = groupName,
                Environment = environment
            }, new JsonMediaTypeFormatter());

            var result = _client.SendAsync(message).Result;

            if (result.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public void RemoveAll()
        {
            var message = HttpMessageRequest.CreateRequest(_baseUrl + "api/config", HttpMethod.Delete);
            var result = _client.SendAsync(message).Result;

            if (result.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public void AddWatch(string key, string urlCallback)
        {
            var message = HttpMessageRequest.CreateRequest(_baseUrl + "api/broadcast", HttpMethod.Post, new BroadCastNotifyRequest
            {
                Name = key,
                GroupName = "",
                EventName = "OnChanged",
                UrlCallback = urlCallback
            }, new JsonMediaTypeFormatter());

            var result = _client.SendAsync(message).Result;

            if (result.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public IEnumerable<BroadCastNotifyRequest> GetWatchers()
        {
            var message = HttpMessageRequest.CreateRequest(_baseUrl + "api/broadcast", HttpMethod.Get);

            var result = _client.SendAsync(message).Result;

            var data = result.Content.ReadAsAsync<IEnumerable<BroadCastNotifyRequest>>().Result.ToList();

            return data;
        }
    }

    
}