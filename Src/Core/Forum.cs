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
    public class Forum : BasePage
    {
        public Forum(IHttpClient httpClient, string forumId, int pageNumber = 1)
            : base(BasePage.PageTypes.Forum, httpClient, forumId, pageNumber)
        {
        }

        public async Task<Forum[]> GetChildForums()
        {
            List<Forum> forums = new List<Forum>();

            var htmlSourcce = await GetHtmlSource();

            string[] forumIds = ExtractChildForumIds(htmlSourcce);

            foreach (var forumId in forumIds)
            {
                forums.Add(new Forum(_httpClient, forumId));
            }

            return forums.ToArray();
        }

        public Forum GotoPage(int pageNumber)
        {
            return new Forum(_httpClient, this.Id, pageNumber);
        }

        public async Task<Thread[]> GetThreads()
        {
            List<Thread> threads = new List<Thread>();

            var html_sourcce = await GetHtmlSource();

            string[] threadIds = ExtractThreadIds(html_sourcce);

            foreach (var threadId in threadIds)
            {
                threads.Add(new Thread(_httpClient, threadId));
            }

            return threads.ToArray();
        }

        private string[] ExtractChildForumIds(string htmlSource)
        {
            List<string> forumIds = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);

            var forumHrefs = doc.DocumentNode.SelectNodes("//ol[@id='forums'] " +
                                                          "//a[starts-with(@href,'forums/') and '/' = substring(@href, string-length(@href)-string-length('/')+1)]");

            foreach (var forumHref in forumHrefs)
            {
                var href = forumHref.Attributes["href"].Value;
                var forumId = href.Replace("/", "").Replace("forums", "");

                forumIds.Add(forumId);
            }

            return forumIds.ToArray();
        }

        private string[] ExtractThreadIds(string htmlSource)
        {
            List<string> threadIds = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);

            var threadHrefs = doc.DocumentNode.SelectNodes("//a[starts-with(@href,'threads/') and '/' = substring(@href, string-length(@href)-string-length('/')+1)]");

            foreach (var threadHref in threadHrefs)
            {
                var href = threadHref.Attributes["href"].Value;
                var threadId = href.Replace("/", "").Replace("threads", "");

                threadIds.Add(threadId);
            }

            threadIds = threadIds.Distinct().ToList();

            return threadIds.ToArray();
        }
    }
}
