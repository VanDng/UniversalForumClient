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
                Post post = ParsePost(messageNode, isRootNode: true);
                posts.Add(post);
            }

            return posts;
        }

        private Post ParsePost(HtmlNode messageNode, bool isRootNode = false)
        {
            var author = messageNode.Attributes["data-author"].Value;

            HtmlNode messageTextNode = null;
            if (isRootNode)
            {
                messageTextNode = messageNode.SelectSingleNode(".//blockquote[contains(@class,'messageText')]");               
            }
            else
            {
                 messageTextNode = messageNode.SelectSingleNode(".//div[contains(@class,'quote')]");
            }

            var contentNodes = messageTextNode.ChildNodes
                                                  .Where(w => !(w.Name == "div" &&
                                                                w.Attributes.Contains("class") &&
                                                                w.Attributes["class"].Value == "messageTextEndMarker"));

            var contents = new List<object>();

            foreach (var contentNode in contentNodes)
            {
                var content = ParseContent(contentNode);

                contents.Add(content);
            }

            var textCombinedContents = CombineTextContent(contents);

            var spaceFilteredContents = FilterUnnecessarySpace(textCombinedContents);

            contents = spaceFilteredContents;

            Post post = new Post(author, contents.ToArray());

            return post;
        }

        private Sploiler ParseSpoiler(HtmlNode spoilerNote)
        {
            var contentNodes = spoilerNote.ChildNodes;

            var contents = new List<object>();

            foreach(var contentNode in contentNodes)
            {
                var content = ParseContent(contentNode);
                contents.Add(content);
            }

            return new Sploiler(contents.ToArray());
        }

        private object ParseContent(HtmlNode contentNode)
        {
            object content = null;

            if (contentNode.Name == "div" && contentNode.Attributes.Contains("data-author"))
            {
                content = ParsePost(contentNode);
            }
            else if (contentNode.Name == "div" && contentNode.Attributes.Contains("class") && contentNode.Attributes["class"].Value.Contains("bbCodeSpoilerContainer"))
            {
                var spoilerNode = contentNode.SelectSingleNode(".//div[contains(@class,'bbCodeSpoilerText')]");
                content = ParseSpoiler(spoilerNode);
            }
            else if (contentNode.Name == "img")
            {
                content = new Image(contentNode.Attributes["src"].Value);
            }
            else if (contentNode.Name == "br")
            {
                content = new Break();
            }
            else
            {
                content = new Text(contentNode.InnerText, contentNode.OuterHtml);
            }

            return content;
        }

        private List<object> CombineTextContent(IEnumerable<object> contents)
        {
            var newContents = new List<object>();

            var textContents = new List<Text>();

            for(int idx = 0; idx < contents.Count(); idx++)
            {
                var content = contents.ElementAt(idx);
                var isTextContent = content as Text == null ? false : true;

                if (isTextContent)
                {
                    textContents.Add((Text)content);
                }
                
                if (idx == contents.Count() -1 ||
                    isTextContent == false)
                {
                    if (textContents.Count > 0)
                    {
                        var plainText = string.Join("", textContents.Select(s => s.PlainText));
                        var markupText = string.Join("", textContents.Select(s => s.MarkupText));

                        var newText = new Text(plainText, markupText);

                        newContents.Add(newText);

                        textContents.Clear();
                    }
                }

                if (isTextContent == false)
                {
                    newContents.Add(content);
                }
            }

            return newContents;
        }

        private List<object> FilterUnnecessarySpace(IEnumerable<object> contents)
        {
            var newContents = new List<object>();

            foreach(var content in contents)
            {
                if (content is Text)
                {
                    var textContent = content as Text;
                    StringBuilder sb = new StringBuilder();

                    var newPlainText = textContent.PlainText;
                    var newMarkupText = textContent.MarkupText;

                    // Start with
                    if (newMarkupText.StartsWith("\n"))
                    {
                        sb.Append("\n");
                    }
                    do
                    {
                        sb.Append("\t");
                    } while (newMarkupText.StartsWith(sb.ToString()));
                    sb.Remove(sb.Length - 1, 1);

                    newPlainText = newPlainText.Remove(0, sb.Length);
                    newMarkupText = newMarkupText.Remove(0, sb.Length);

                    // End with
                    sb.Clear();
                    do
                    {
                        sb.Append("\t");
                    } while (newMarkupText.EndsWith(sb.ToString()));
                    sb.Remove(sb.Length - 1, 1);

                    sb.Insert(0, "\n");
                    if (newMarkupText.EndsWith(sb.ToString()) == false)
                    {
                        sb.Remove(0, 1);
                    }

                    if (sb.Length > 0)
                    {
                        newPlainText = newPlainText.Remove(newPlainText.Length - sb.Length);
                        newMarkupText = newMarkupText.Remove(newMarkupText.Length - sb.Length);
                    }

                    var newTextContent = new Text(newPlainText, newMarkupText);
                    newContents.Add(newTextContent);
                }
                else
                {
                    newContents.Add(content);
                }
            }

            return newContents;
        }
    }
}
