using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniversalForumClient.Http;

namespace TestProject1
{
    public abstract class TestBase
    {
        protected HttpClientStub _httpClientStub;
        protected HttpClient _httpClient;

        protected string _testDataDir;

        public TestBase()
        {
            _httpClientStub = new HttpClientStub();
            _httpClient = new HttpClient(_httpClientStub);

            _testDataDir = Path.Combine(Utility.SolutionPath(), "Test", "TestData");
        }

        public virtual void Dispose()
        {
            _httpClientStub.Dispose();
            _httpClient.Dispose();
        }
    }
}
