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

        public async Task<List<Post>> GetPosts()
        {
            List<Post> posts = new List<Post>();

            var htmlSource = await GetHtmlSource();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlSource);

            var messageList = htmlDocument.DocumentNode.SelectNodes("//ol[@id='messageList']//li");

            foreach(var messageNode in messageList)
            {
                var author = messageNode.Attributes["data-author"].Value;

                var messageTextNode = messageNode.SelectSingleNode(".//blockquote[contains(@class,'messageText')]");

                var contentNodes = messageTextNode.ChildNodes
                                                  .Where(w => !(w.Name == "div" && w.Attributes["class"].Value == "messageTextEndMarker") &&
                                                              !(w.Name == "br"));
                Post post = new Post(author, contentNodes.ToArray());
  
                posts.Add(post);
            }

            return posts;
        }
    }
}
