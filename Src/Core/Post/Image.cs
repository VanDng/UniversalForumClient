using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Image
    {
        public string Url { get; private set; }

        public Image(string url)
        {
            Url = url;
        }
    }
}
