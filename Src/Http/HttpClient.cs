using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
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

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return ResponseFilter(_httpClient.GetAsync(uri));
            
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
            return ResponseFilter(_httpClient.PostAsync(uri, content));
        }

        private Task<HttpResponseMessage> ResponseFilter(Task<HttpResponseMessage> responseTask)
        {
            var task = Task.Run(async () =>
            {
                var orgResponseMessage = await responseTask;

                IEnumerable<string> contentTypes = new string[] { };
                if (orgResponseMessage.Content.Headers.TryGetValues("Content-Type", out contentTypes))
                {
                    if (contentTypes.Count() > 0 &&
                        contentTypes.Any(a => a == @"text\html"))
                    {
                        var orgResponseContent = await orgResponseMessage.Content.ReadAsStringAsync();

                        var newResponseContent = HtmlHelper.RemoveInsignificantHtmlWhiteSpace(orgResponseContent);

                        var byteArray = Encoding.UTF8.GetBytes(newResponseContent);
                        var byteArrayContent = new ByteArrayContent(byteArray);
                        orgResponseMessage.Content = byteArrayContent;
                    }
                }

                return orgResponseMessage;
            });

            return task;
        }
    }
}
