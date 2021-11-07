using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UniversalForumClient.Http;

namespace UniversalForumClient.Core
{
    public class Forum
    {
        private IHttpClient _httpClient;

        // An ID can be "some-thing-special.99" or just "99",
        // it's only SEO matter, here we don't need to care about it
        public string Id { get; private set; }

        public Forum(IHttpClient httpClient, string forumId)
        {
            _httpClient = httpClient;
            
            Id = forumId;
        }

        public async Task<Forum[]> GetChildForums()
        {
            List<Forum> forums = new List<Forum>();

            var htmlSourcce = await FetchHtmlSource(1);

            string[] forumIds = ExtractChildForumIds(htmlSourcce);

            foreach (var forumId in forumIds)
            {
                forums.Add(new Forum(_httpClient, forumId));
            }

            return forums.ToArray();
        }

        public async Task<int> GetTotalPage()
        {
            // TODO
            return 1;
        }

        public async Task<Thread[]> GetThreads(int pageIndex)
        {
            List<Thread> threads = new List<Thread>();

            var html_sourcce = await FetchHtmlSource(pageIndex);

            string[] threadIds = ExtractThreadIds(html_sourcce);

            foreach (var threadId in threadIds)
            {
                threads.Add(new Thread(_httpClient, threadId));
            }

            return threads.ToArray();
        }

        private string[] ExtractChildForumIds(string html_source)
        {
            List<string> forumIds = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_source);

            var forumNode = doc.DocumentNode.SelectSingleNode("//ol[@id='forums']");

            var forumHrefs = forumNode.SelectNodes("//a[starts-with(@href='forums/')]" +
                                                      "[ends-with(@href='/')");

            foreach (var forumHref in forumHrefs)
            {
                Debug.WriteLine(forumHref.InnerText);
            }
            return forumIds.ToArray();
        }

        private string[] ExtractThreadIds(string html_source)
        {
            List<string> threadIds = new List<string>();

            // TODO

            return threadIds.ToArray();
        }

        private async Task<string> FetchHtmlSource(int pageIndex)
        {
            string htmlSource = string.Empty;

            string uri = UriManager.ForumUri(Id, pageIndex);

            var checkResponse = await _httpClient.GetAsync(uri);
            if (checkResponse.IsSuccessStatusCode)
            {
                htmlSource = await checkResponse.Content.ReadAsStringAsync();
            }

            return htmlSource;
        }
    }
}
