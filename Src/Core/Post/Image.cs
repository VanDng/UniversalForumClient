using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UniversalForumClient.Http;

namespace UniversalForumClient.Core
{
    public class Image
    {
        public string Source { get; private set; }

        private IHttpClient _httpClient;

        public Image(string url, IHttpClient httpClient)
        {
            Source = url;

            _httpClient = httpClient;
        }

        public async Task SaveAs(string path)
        {
            // Same BaseAddress
            if (_httpClient.BaseAddress.IsBaseOf(new Uri(Source)))
            {
                string uri = Source.Replace(_httpClient.BaseAddress.ToString(), string.Empty);

                var response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var imageContent = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(path, imageContent);
                }
            }
            else
            {
                var w = new WebClient();
                w.DownloadFile(Source, path);
            }
        }
    }
}
