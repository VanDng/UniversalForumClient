using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using UniversalForumClient.Core;
using UniversalForumClient.Extension;
using UniversalForumClient.Http;
using Xunit;

namespace TestProject1
{
    public class TestThread : TestBase
    {
        private Thread _thread;

        public TestThread()
        {
            _httpClientStub = new HttpClientStub();
            _httpClient = new HttpClient(_httpClientStub);

            _thread = new Thread(_httpClient, "99");
        }

        private void SerializePosts(List<Post> posts)
        {
            var dir = Utility.SolutionPath() + @"\Test\TestResult\Gvn";

            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }

            string jsonString = posts.Serialize();

            //JsonSerializerOptions jso = new JsonSerializerOptions();
            //jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            //string jsonString = JsonSerializer.Serialize(posts, jso);

            string serializeOutputFile = string.Format(@"{0}\{1}.json", dir
                                                                 , DateTime.Now.ToString("yyyyMMddHHmmss"));

            File.WriteAllText(serializeOutputFile, jsonString);
        }

        [Fact]
        public async void GetPosts()
        {
            var testDataFilePath = string.Format(@"{0}\{1}\{2}", _testDataDir, "Gvn", "thread_html_source.html");
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
            Assert.Equal(expectedContents1st.First().ToString(), content1st.Source);

            var content2nd = contents1st.Last() as Text;
            Assert.NotNull(content2nd);
            Assert.Equal(expectedContents1st.Last().ToString(), content2nd.PlainText);
        }

        [Fact]
        public async void GetPosts_Spoiler()
        {
            var testDataFilePath = string.Format(@"{0}\{1}\{2}", _testDataDir, "Gvn", "thread_spoiler_html_source.html");
            var html_sourcce = File.ReadAllText(testDataFilePath);
            _httpClientStub.SetHttpResponse(html_sourcce);

            var posts = await _thread.GetPosts();

            // Post count
            int expectedPostCount = 2;
            Assert.Equal(posts.Count, expectedPostCount);

            // Post content
            // 1st post
            string expectedImage1st = "data/attachments/278/278049-8acb73f2f0067d4d343c8e392b94d92175675f245.jpg";
            var contents1st = posts.First().Contents;
            var img1st = (((contents1st.First() as Spoiler).Contents.First() as Hyperlink).Contents.First() as Image).Source;
            Assert.Equal(expectedImage1st, img1st);

            // last post
            string expectedImageLast = "data/attachments/278/278654-771dc7f070c9354365f1290bd54fb7541ad8b6b65.jpg";
            var contentsLast = posts.Last().Contents;
            var imgLast = (((contentsLast.Last() as Spoiler).Contents.Last() as Hyperlink).Contents.Last() as Image).Source;
            Assert.Equal(expectedImageLast, imgLast);
        }
    }
}
