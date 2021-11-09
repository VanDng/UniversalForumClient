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
                Post post = ParsePost(messageNode, true);
                posts.Add(post);
            }

            return posts;
        }

        private Post ParsePost(HtmlNode messageNode, bool isRootPost = false)
        {
            var author = messageNode.Attributes["data-author"].Value;

            HtmlNode messageTextNode = null;
            if (isRootPost)
            {
                messageTextNode = messageNode.SelectSingleNode(".//blockquote[contains(@class,'messageText')]");               
            }
            else
            {
                messageTextNode = messageNode.SelectSingleNode(".//div[contains(@class,'quote')]");
            }

            var contentNodes = messageTextNode.ChildNodes
                                                  .Where(w => !(w.Name == "div" && w.Attributes["class"].Value == "messageTextEndMarker") &&
                                                              !(w.Name == "br"));

            var contents = new List<object>();

            foreach (var contentNode in contentNodes)
            {
                object content = null;

                if (contentNode.Name == "div" && contentNode.Attributes.Contains("data-author"))
                {
                    content = ParsePost(contentNode);
                }
                else if (contentNode.Name == "img")
                {
                    content = new Image(contentNode.Attributes["src"].Value);
                }
                else
                {
                    content = new Text(contentNode.InnerText, contentNode.OuterHtml);
                }

                contents.Add(content);
            }

            Post post = new Post(author, contents.ToArray());

            return post;
        }
    }
}
