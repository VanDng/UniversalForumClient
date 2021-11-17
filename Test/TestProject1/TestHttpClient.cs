using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.Http;
using Xunit;

namespace TestProject1
{
    public class TestHttpClient
    {
        [Fact]
        public async Task MassRequestSameUri()
        {
            var stubClient = new HttpClientStub();
            var client = new HttpClient(stubClient);

            var response = await client.GetAsync("abc");
            var responseStatus = response.StatusCode;

            Assert.Equal(System.Net.HttpStatusCode.OK, responseStatus);
        }
    }
}
