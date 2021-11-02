using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniversalForumClient
{
    public class ForumClient : IDisposable
    {
        private string _homePageUrl;
        private HttpClient _httpClient;
        private User _user { get; set; }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        public ForumClient(string homePageUrl)
        {
            _homePageUrl = homePageUrl;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_homePageUrl);

            _user = null;
        }

        public async Task<bool> Login(string userName, string password)
        {
            bool isSuccess = false;

            _user = new ForumConnector.User();
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

                    if (responseContent.Contains("Signed in as"))
                    {
                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }

        public Forum GetForum(string forumId = null)
        {
            Forum forum = new Forum(_httpClient, forumId);
            return forum;
        }

        public Thread GetThread(string threadId)
        {
            Thread thread = new Thread(_httpClient, threadId);
            return thread;
        }
    }
}
