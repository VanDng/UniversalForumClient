using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniversalForumClient.Core;
using UniversalForumClient.Http;
using Xunit;

namespace TestProject1
{
    public class TestThread : IDisposable
    {
        private HttpClientStub _httpClientStub;
        private HttpClient _httpClient;
        private Thread _thread;

        public TestThread()
        {
            _httpClientStub = new HttpClientStub();
            _httpClient = new HttpClient(_httpClientStub);

            _thread = new Thread(_httpClient, "99");
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
        public async void GetPosts()
        {
            

            var testDataFilePath = TestDataPath("thread_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var posts = await _thread.GetPosts();

            // Post count
            int expectedPostCount = 20;
            Assert.Equal(posts.Count, expectedPostCount);

            // Author
            string[] expectedAuthors =
            {
                "ThanatosII",
                "Nô.",
                "Achiles88",
                "namff",
                "urusei",
                "Nô.",
                "MCGH",
                "namff",
                "hgiasac",
                "Aiden Fox Pearce",
                "angel4321",
                "T1nhLaG1",
                "zondaR",
                "scuuby",
                "Warfield",
                "squall9588",
                "bivboi",
                "Hắc Ma",
                "Vouu2",
                "scuuby"
            };
            var authors = posts.Select(s => s.Author);
            Assert.All(expectedAuthors, a => Assert.Contains(a, authors));

            // Post content

        }
    }
}
