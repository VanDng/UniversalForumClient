using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.Http;

namespace TestProject1
{
    public class HttpClientStub : IHttpClient
    {
        private HttpResponseMessage _httpResponseMessage;

        public Uri BaseAddress { get; set; }

        public HttpClientStub()
        {
            RefreshHttpReponse();
        }

        public void Dispose()
        {
       
        }

        public void RefreshHttpReponse()
        {
            _httpResponseMessage = new HttpResponseMessage();
            _httpResponseMessage.StatusCode = HttpStatusCode.OK;
            _httpResponseMessage.Content = new ByteArrayContent(new byte[] { });
        }

        public void SetHttpResponse(HttpStatusCode statusCode)
        {
            _httpResponseMessage.StatusCode = statusCode;
        }

        public void SetHttpResponse(string content)
        {
            var byteArray = Encoding.UTF8.GetBytes(content);
            var byteArrayContent = new ByteArrayContent(byteArray);

            _httpResponseMessage.Content = byteArrayContent;
        }

        public void SetHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            var t = new Task<HttpResponseMessage>(() =>
            {
                return _httpResponseMessage;
            });

            t.Start();

            return t;
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            var t =  new Task<HttpResponseMessage>(() =>
            {
                return _httpResponseMessage;
            });

            t.Start();

            return t;
        }
    }
}
