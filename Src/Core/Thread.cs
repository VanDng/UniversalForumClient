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
                var post = ParsePost(messageNode);

                posts.Add(post);
            }

            return posts;
        }

        private Post ParsePost(HtmlNode rootNode)
        {
            var author = rootNode.Attributes["data-author"].Value;

            var messageTextNode = rootNode.SelectSingleNode(".//blockquote[contains(@class,'messageText')]");
            var contents = ParseContents(messageTextNode);

            Post post = new Post(author, contents);

            return post;
        }

        private object[] ParseContents(HtmlNode messageNode)
        {
            var contentNodes = messageNode.ChildNodes
                                                    .Where(w => !(w.Name == "div" &&
                                                                w.Attributes.Contains("class") &&
                                                                w.Attributes["class"].Value == "messageTextEndMarker"));

            var contents = new List<object>();

            foreach (var contentNode in contentNodes)
            {
                var content = ParseContent(contentNode);
                contents.Add(content);
            }

            contents = FilterUnnecessarySpace(contents);

            contents = FilterUnnecessaryContent(contents);

            contents = CombineTextContent(contents);

            return contents.ToArray();
        }

        private object ParseContent(HtmlNode contentNode)
        {
            object content = null;

            if (contentNode.Name == "div" && contentNode.Attributes.Contains("class") && contentNode.Attributes["class"].Value.Contains("bbCodeQuote"))
            {
                var author = string.Empty;

                if (contentNode.Attributes.Contains("data-author"))
                {
                    author = contentNode.Attributes["data-author"].Value;
                }

                var messageTextNode = contentNode.SelectSingleNode(".//div[contains(@class,'quote')]");
                var childContents = ParseContents(messageTextNode);

                content = new Quote(author, childContents);
            }
            else if (contentNode.Name == "div" && contentNode.Attributes.Contains("class") && contentNode.Attributes["class"].Value.Contains("bbCodeSpoilerContainer"))
            {
                var spoilerNode = contentNode.SelectSingleNode(".//div[contains(@class,'bbCodeSpoilerText')]");
                var childContents = ParseContents(spoilerNode);
                content = new Spoiler(childContents);
            }
            else if (contentNode.Name == "img")
            {
                content = new Image(contentNode.Attributes["src"].Value);
            }
            else if (contentNode.Name == "br")
            {
                content = new Break();
            }
            else if (contentNode.Name == "a")
            {
                var href = contentNode.Attributes["href"].Value;
                var childContents = ParseContents(contentNode);
                content = new Hyperlink(href, childContents);
            }
            else if (contentNode.Name == "#text")
            {
                content = new Text(contentNode.InnerText, contentNode.OuterHtml);
            }
            else if (contentNode.Name == "div" && contentNode.Attributes.Contains("class") == false && contentNode.Attributes.Contains("style"))
            {
                var style = contentNode.Attributes["style"].Value;
                var childContents = ParseContents(contentNode);
                content = new StyleContainer(style, childContents);
            }
            else
            {
                content = new Undefined(contentNode.OuterHtml);
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
                    while (true)
                    {
                        int preLength = newPlainText.Length;

                        sb.Clear();
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

                        if (newPlainText.Length == preLength)
                        {
                            break;
                        }
                    }

                    // End with
                    while (true)
                    {
                        int preLength = newPlainText.Length;

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

                        if (newPlainText.Length == preLength)
                        {
                            break;
                        }
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

        private List<object> FilterUnnecessaryContent(IEnumerable<object> contents)
        {
            var newObjects = new List<object>();

            foreach(var content in contents)
            {
                if (content is Text)
                {
                    var textContent = content as Text;

                    if (textContent.PlainText == "&#8203;")
                    {
                        continue;
                    }
                }
                 
                newObjects.Add(content);
            }

            return newObjects;
        }
    }
}
