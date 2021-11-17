using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class TestStub
    {
        private HttpClientStub _httpClient;

        public TestStub()
        {
            _httpClient = new HttpClientStub();
            _httpClient.RefreshHttpReponse();
        }

        [Fact]
        public async Task HttpClientStubGetAsync()
        {
            string expectedHtmlSource = "Hello stub world!";

            _httpClient.SetHttpResponse(expectedHtmlSource);

            var htmlSource = string.Empty;
            var checkResponse = await _httpClient.GetAsync(string.Empty);
            if (checkResponse.IsSuccessStatusCode)
            {
                htmlSource = checkResponse.Content.ReadAsStringAsync().Result;
            }

            Assert.Equal(expectedHtmlSource, htmlSource);
        }

        [Fact]
        public async Task HttpClientStubPostAsync()
        {
            string expectedHtmlSource = "Hello stub world!";

            _httpClient.SetHttpResponse(expectedHtmlSource);

            var htmlSource = string.Empty;
            var checkResponse = await _httpClient.PostAsync(string.Empty, null);
            if (checkResponse.IsSuccessStatusCode)
            {
                htmlSource = checkResponse.Content.ReadAsStringAsync().Result;
            }

            Assert.Equal(expectedHtmlSource, htmlSource);
        }
    }
}
