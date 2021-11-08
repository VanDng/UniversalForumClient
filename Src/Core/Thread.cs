using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient;
using UniversalForumClient.Http;

namespace UniversalForumClient.Core
{
    public class Thread : BasePage
    {
        public Thread(IHttpClient httpClient, string threadId, int pageNumber = 1)
            : base(BasePage.PageTypes.Thread, httpClient, threadId, pageNumber)
        {
        }

        public Thread GotoPage(int pageNumber)
        {
            return new Thread(_httpClient, Id, pageNumber);
        }

        public async Task<Post[]> GetPosts()
        {
            List<Post> posts = new List<Post>();

            var html_source = await GetHtmlSource();

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
    }
}
