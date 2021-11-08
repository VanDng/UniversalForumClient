using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalForumClient.Http;

namespace UniversalForumClient.Core
{
    public abstract class BasePage
    {
        public enum PageTypes
        {
            Forum,
            Thread
        }

        protected IHttpClient _httpClient;
        private PageTypes _pageType;

        public string Id { get; private set; }

        public int PageNumber { get; private set; }

        public BasePage(PageTypes pageType, IHttpClient httpClient, string id, int pageNumber)
        {
            _pageType = pageType;
            _httpClient = httpClient;

            Id = id;
            PageNumber = pageNumber;
        }

        public async Task<int> GetTotalPage()
        {
            var htmlSourcce = await GetHtmlSource();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlSourcce);

            var nav = doc.DocumentNode.SelectSingleNode("//div[@class='PageNav']");

            var totalPage = -1;
            int.TryParse(nav.Attributes["data-last"].Value, out totalPage);

            return totalPage;
        }

        protected async Task<string> GetHtmlSource()
        {
            string htmlSource = string.Empty;

            string uri = string.Empty;

            switch (_pageType)
            {
                case PageTypes.Thread:
                    uri = UriManager.ThreadUri(Id, PageNumber);
                    break;

                case PageTypes.Forum:
                    uri = UriManager.ForumUri(Id, PageNumber);
                    break;

                default:
                    break;
            }

            var checkResponse = await _httpClient.GetAsync(uri);
            if (checkResponse.IsSuccessStatusCode)
            {
                htmlSource = await checkResponse.Content.ReadAsStringAsync();
            }

            return htmlSource;
        }
    }
}
