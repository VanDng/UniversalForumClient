using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UniversalForumClient.Core;
using UniversalForumClient.Exceptions;
using Xunit;

namespace TestProject1
{
    public class TestForumClient : TestBase, IDisposable
    {
        private ForumClient _forumClient;

        public TestForumClient()
        {
            _forumClient = new ForumClient("http://gamevn.com", _httpClient);
        }

        public override void Dispose()
        {
            _forumClient.Dispose();

        }

        #region Offline data

        [Fact]
        public async Task Offline_Login_Failure()
        {
            var testDataFilePath = string.Format(@"{0}\{1}\{2}", _testDataDir, "Gvn", "login_failure.html");
            var testData = File.ReadAllText(testDataFilePath);

            _httpClientStub.SetHttpResponse(testData);

            var loginStatus = await _forumClient.Login("anything", "anything");

            Assert.False(loginStatus);
        }

        [Fact]
        public async Task Offline_Login_Success()
        {
            var testDataFilePath = string.Format(@"{0}\{1}\{2}", _testDataDir, "Gvn", "login_success.html");
            var testData = File.ReadAllText(testDataFilePath);

            _httpClientStub.SetHttpResponse(testData);

            var loginStatus = await _forumClient.Login("anything", "anything");

            Assert.True(loginStatus);
        }

        [Fact]
        public void Goto_Invalid_Thread()
        {
            var exception = Assert.Throws<ThreadIdNotValid>(() =>
            {
                _forumClient.GotoThread(null);
            });
        }

        #endregion Offline data

        #region Online data

        [Fact]
        public async Task Login_Success()
        {

        }

        #endregion Online data
    }
}
