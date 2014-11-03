using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace CentralConfig.Client
{
    public static class HttpMessageRequest
    {
        public static HttpRequestMessage CreateRequest(string url, HttpMethod method, MediaTypeWithQualityHeaderValue mthv = null)
        {
            var request = new HttpRequestMessage { RequestUri = new Uri(url) };

            if (mthv == null)
            {
                mthv = new MediaTypeWithQualityHeaderValue("application/json");
            }

            request.Headers.Accept.Add(mthv);
            request.Method = method;

            return request;
        }

        public static HttpRequestMessage CreateRequest<T>(string url, HttpMethod method, T content, MediaTypeFormatter formatter, MediaTypeWithQualityHeaderValue mthv = null) where T : class
        {
            var request = CreateRequest(url, method, mthv);
            request.Content = new ObjectContent<T>(content, formatter);

            return request;
        }
    }
}