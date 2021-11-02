using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ForumConnector
{
    public class Thread
    {
        private HttpClient _httpClient;

        public string Id { get; private set; }

        public Thread(HttpClient httpClient, string threadId)
        {
            _httpClient = httpClient;
            Id = threadId;
        }

        public async Task<int> GetTotalPage()
        {
            int totalPage = 0;

            var html_source = await FetchHtmlSource(1);

            // TODO

            return totalPage;
        }

        public async Task<Post[]> GetPosts(int pageIndex)
        {
            List<Post> posts = new List<Post>();

            var html_source = await FetchHtmlSource(pageIndex);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html_source);
            var postList = htmlDocument.GetElementbyId("//messageList");

            foreach(var postItem in postList.ChildNodes)
            {
                Post post = Post.ParseFromHtml(postItem.InnerHtml);
                posts.Add(post);
            }
            
            return posts.ToArray();
        }

        private async Task<string> FetchHtmlSource(int pageIndex)
        {
            string html_source = string.Empty;

            string uri = string.Format("threads/{0}/page-{1}", Id, pageIndex);

            var checkResponse = await _httpClient.GetAsync(uri);
            if (checkResponse.IsSuccessStatusCode)
            {
                html_source = await checkResponse.Content.ReadAsStringAsync();
            }

            return html_source;
        }
    }
}
