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
            // 1st post
            object[] expectedContents1st = new object[]
            {
                "https://scontent.fsgn5-10.fna.fbcdn.net/v/t1.6435-9/253959364_411483480532566_4715033918823503796_n.jpg?_nc_cat=110&amp;ccb=1-5&amp;_nc_sid=825194&amp;_nc_ohc=ohkQ0vBA3YAAX94_o5A&amp;_nc_ht=scontent.fsgn5-10.fna&amp;oh=81468ea52407d06fc566d8d401ae39c9&amp;oe=61AC97D5",
                "\"                  chi la de\"test\"ma              thoi              \""
            };
            var contents1st = posts.First().Contents;
            var content1st = contents1st.First() as Image;
            Assert.NotNull(content1st);
            Assert.Equal(expectedContents1st.First().ToString(), content1st.Url);

            var content2nd = contents1st.Last() as Text;
            Assert.NotNull(content2nd);
            Assert.Equal(expectedContents1st.Last().ToString(), content2nd.PlainText);
        }

        [Fact]
        public async void GetPosts_Spoiler()
        {
            var testDataFilePath = TestDataPath("thread_spoiler_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var posts = await _thread.GetPosts();

            // Post count
            int expectedPostCount = 20;
            Assert.Equal(posts.Count, expectedPostCount);

            // Post content
            // 1st post
            object[] expectedContents1st = new object[]
            {
                "http://gamevn.com/data/attachments/278/278049-8acb73f2f0067d4d343c8e392b94d921.jpg",
                "\"                  chi la de\"test\"ma              thoi              \""
            };
            var contents1st = posts.First().Contents;
            var sploier1st = contents1st.First() as Sploiler;
            Assert.NotNull(sploier1st);
            var image1st = sploier1st.Contents.First() as Image;
            Assert.NotNull(image1st);
            Assert.Equal(expectedContents1st.First().ToString(), image1st.Url);
        }
    }
}
