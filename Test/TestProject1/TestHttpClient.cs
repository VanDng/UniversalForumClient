using System;
using System.Collections.Generic;
using System.Text;
using UniversalForumClient.Http;
using Xunit;

namespace TestProject1
{
    public class TestHttpClient
    {
        [Fact]
        public void MassRequestSameUri()
        {
            var client = new HttpClientWrapper();
            var response = client.GetAsync("abc").Result;
            Assert.Equal(200, (int)response.StatusCode);
        }
    }
}
