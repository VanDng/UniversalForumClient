using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.Core;
using UniversalForumClient.Http;
using Xunit;

namespace TestProject1
{
    public class TestForum
    {
        private HttpClientStub _httpClientStub;
        private HttpClient _httpClient;
        private Forum _forum;

        public TestForum()
        {
            _httpClientStub = new HttpClientStub();
            _httpClient = new HttpClient(_httpClientStub);

            _forum = new Forum(_httpClient, "99");
        }

        [Fact]
        public async Task GetChildForums()
        {
            string[] expectedChildForumIds =
            {
                "covid",
                "gamevn-wiki.504",
                "thu-gian-express-ban-tin-cuoi-ngay.339",
                "an-choi-tiec-tung.85"
            };

            var testDataFilePath = Utility.SolutionPath() + @"\SampleData\forum_html_source.html";
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var childForums = await _forum.GetChildForums();
            var childForumNames = childForums.Select(s => s.Id);

            Assert.All(expectedChildForumIds, id => Assert.Contains(id, childForumNames));
        }
    }
}
