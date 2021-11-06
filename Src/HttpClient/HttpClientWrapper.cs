using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace UniversalForumClient.HttpClient
{
    public class HttpClientWrapper : IHttpClient, IDisposable
    {
        private System.Net.Http.HttpClient _httpClient;

        public Uri BaseAddress
        {
            get
            {
                return _httpClient.BaseAddress;
            }

            set
            {
                _httpClient.BaseAddress = value;
            }
        }

        public HttpClientWrapper()
        {
            _httpClient = new System.Net.Http.HttpClient();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return _httpClient.GetAsync(uri);
        }

        public Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
        {
            return _httpClient.PostAsync(uri, content);
        }
    }
}
