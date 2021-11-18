using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace UniversalForumClient.Http
{
    public class HttpClientWrapper : IHttpClient, IDisposable
    {
        private readonly System.Net.Http.HttpClient _httpClient;

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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new System.Net.Http.HttpClient(handler);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return _httpClient.GetAsync(uri);
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }
    }
}