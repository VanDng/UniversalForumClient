using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UniversalForumClient.Http
{
    public interface IHttpClient : IDisposable
    {
        public Uri BaseAddress { get; set; }

        public Task<HttpResponseMessage> GetAsync(string uri);

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}
