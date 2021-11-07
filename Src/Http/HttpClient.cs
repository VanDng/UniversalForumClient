using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MSHttpClient = System.Net.Http.HttpClient;

namespace UniversalForumClient.Http
{
    public class HttpClient : IHttpClient, IDisposable
    {
        public bool RequestControl {get; set; }

        private IHttpClient _httpClient;
        private RequestManager _requestManager;

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

        public HttpClient(IHttpClient httpClient = null)
        {
            if (httpClient != null)
            {
                _httpClient = httpClient;
            }
            else
            {
                _httpClient = new HttpClientWrapper();
            }
            
            _requestManager = new RequestManager();
            RequestControl = true;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await _httpClient.GetAsync(uri);
            
            // if (RequestControl)
            // {
            //     var request = new Request(uri, Request.Methods.GET);

            //     if (_requestManager.IsRequestAllowed(request))
            //     {
            //         var requestTask = _httpClient.GetAsync(uri);

            //         if (_requestManager.Contains(request))
            //         {
            //             _requestManager.AddRequest(request);
            //         }

            //         _requestManager.UpdateRequestDetail(request, true);

            //         var requestResponse = await requestTask;
                    
            //         _requestManager.UpdateRequestDetail(request, false);
            //         _requestManager.UpdateRequestDetail(request, requestResponse);

            //         return requestResponse;
            //     }
            //     else
            //     {
            //         return _requestManager.GetLastResponse(request);
            //     }
            // }
            // else
            // {
            //     return await _httpClient.GetAsync(uri);
            // }
        }

        public Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
        {
            return _httpClient.PostAsync(uri, content);
        }
    }
}
