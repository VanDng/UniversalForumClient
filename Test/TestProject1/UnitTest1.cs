using System;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        private HttpClientStub _httpClient;

        public UnitTest1()
        {
            _httpClient = new HttpClientStub();
            _httpClient.RefreshHttpReponse();
        }

        [Fact]
        public void ParseChildForums()
        {
            _httpClient.SetHttpResponse("Hello stub world!");

            var htmlSource = string.Empty;
            var checkResponse = _httpClient.GetAsync("abc").Result;
            if (checkResponse.IsSuccessStatusCode)
            {
                htmlSource = checkResponse.Content.ReadAsStringAsync().Result;
            }

            string x = htmlSource;
        }
    }
}
