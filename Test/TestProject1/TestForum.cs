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
    public class TestForum : IDisposable
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

        public void Dispose()
        {
            _httpClientStub.Dispose();
            _httpClient.Dispose();
        }

        private string TestDataPath(string relativeFilePath)
        {
            return Utility.SolutionPath() + @"\Test\TestData\Gvn\" + relativeFilePath;
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

            var testDataFilePath = TestDataPath("forum_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var childForums = await _forum.GetChildForums();
            var childForumIds = childForums.Select(s => s.Id);

            Assert.All(expectedChildForumIds, id => Assert.Contains(id, childForumIds));
            Assert.Equal(expectedChildForumIds.Count(), childForumIds.Count());
        }

        [Fact]
        public async Task GetTotalPage()
        {
            int expectedTotalPage = 647;

            var testDataFilePath = TestDataPath("forum_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var totalPage = await _forum.GetTotalPage();

            Assert.Equal(expectedTotalPage, totalPage);
        }

        [Fact]
        public async Task GetThreads()
        {
            string[] expectedSomeThreads =
            {
                "hinh-thu-gian-v66-cung-nhau-day-lui-dich-covid-19-sfw-only-xem-page-1.1423220",
                "dota2-congratulations-to-the-winners-of-the-international-10-team-spirit.1476161",
                "hoi-nhung-nguoi-me-kshow-kpop-kdrama-va-nhung-thu-lien-quan-den-han-xeng.1416391",
                "co-ai-con-choi-lien-quan-ko.1456064",
                "gamevn-mens-fashion-noi-quy-ong-lam-dep.1522965"
            };

            var testDataFilePath = TestDataPath("forum_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var threads = await _forum.GetThreads();
            var threadIds = threads.Select(s => s.Id);

            Assert.All(expectedSomeThreads, id => Assert.Contains(id, threadIds));
        }
    }
}
