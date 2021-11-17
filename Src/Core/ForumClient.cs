using UniversalForumClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.Http;
using System.Net.Http;
using HttpClient = UniversalForumClient.Http.HttpClient;

namespace UniversalForumClient.Core
{
    public class ForumClient : IDisposable
    {
        private string _homePageUrl;
        private IHttpClient _httpClient;
        private User _user { get; set; }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        public ForumClient(string homePageUrl, IHttpClient httpClient = null)
        {
            _homePageUrl = homePageUrl;

            if (httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            else
            {
                _httpClient = httpClient;
            }

            _httpClient.BaseAddress = new Uri(_homePageUrl);

            _user = null;
        }

        public async Task<bool> Login(string userName, string password)
        {
            bool isSuccess = false;

            _user = new User();
            _user.UserName = userName;
            _user.Password = password;

            var checkResponse = await _httpClient.GetAsync("");
            if (checkResponse.IsSuccessStatusCode)
            {
                string url = string.Format("login/login");

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("login", _user.UserName), 
                    new KeyValuePair<string, string>("register", "0"),
                    new KeyValuePair<string, string>("password", _user.Password),
                    new KeyValuePair<string, string>("remember", "1"),
                    new KeyValuePair<string, string>("cookie_check", "1"),
                    new KeyValuePair<string, string>("redirect", "/"),
                    new KeyValuePair<string, string>("_xfToken", "")
                });
                var loginResponse = await _httpClient.PostAsync(url, formContent);

                if (loginResponse.IsSuccessStatusCode)
                {
                    var responseContent = await loginResponse.Content.ReadAsStringAsync();

                    if (responseContent.Contains("id=\"AccountMenu\""))
                    {
                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }

        public Forum GotoForum(string forumId = null, int pageNumber = 1)
        {
            Forum forum = new Forum(_httpClient, forumId, pageNumber);
            return forum;
        }

        public Thread GotoThread(string threadId, int pageNumber = 1)
        {
            Thread thread = new Thread(_httpClient, threadId, pageNumber);
            return thread;
        }
    }
}
