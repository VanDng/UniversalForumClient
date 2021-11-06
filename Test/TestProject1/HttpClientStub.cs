using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.HttpClient;

namespace TestProject1
{
    class HttpClientStub : IHttpClient
    {
        private HttpResponseMessage _httpResponseMessage;

        public Uri BaseAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public HttpClientStub()
        {
            _httpResponseMessage = new HttpResponseMessage();
        }

        public void Dispose()
        {
       
        }

        public void RefreshHttpReponse()
        {
            _httpResponseMessage = new HttpResponseMessage();
            _httpResponseMessage.StatusCode = HttpStatusCode.OK;
        }

        public void SetHttpReponse(HttpStatusCode statusCode)
        {
            _httpResponseMessage.StatusCode = statusCode;
        }

        public void SetHttpResponse(string content)
        {
            var byteArray = Encoding.ASCII.GetBytes(content);
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
