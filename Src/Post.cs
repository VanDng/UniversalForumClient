using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalForumClient
{
    public class Post
    {
        public string Author { get; private set; }

        public bool IsQuote { get; private set; }

        public object[] Content { get; private set; }

        private Post()
        {
            
        }

        static public Post ParseFromHtml(string html_source)
        {
            Post post = null;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html_source);

            var blockQuote = htmlDocument.DocumentNode.SelectSingleNode("//blockquote");
            var content = ParseContentFromHtml(blockQuote);

            post = new Post();
            post.IsQuote = false;
            post.Content = content;

            return post;
        }

        static private object[] ParseContentFromHtml(HtmlNode blockQuoteNode)
        {
            List<object> content = new List<object>();

            foreach (var childNode in blockQuoteNode.ChildNodes)
            {
                if (childNode.NodeType == HtmlNodeType.Text)
                {
                    string text = childNode.InnerText;

                    content.Add(text);
                }
                else if (childNode.Attributes["class"].Value.Contains("bbCodeQuote") &&
                            childNode.Attributes.Contains("data-author"))
                {
                    Post quote = new Post();
                    quote.Author = childNode.Attributes["data-author"].Value;
                    quote.IsQuote = true;
                    quote.Content = ParseContentFromHtml(childNode.SelectSingleNode("//div[contains(@class, 'quote')]"));

                    content.Add(quote);
                }
            }

            return content.ToArray();
        }
    }
}
