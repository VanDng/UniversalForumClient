using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Image
    {
        public string Source { get; private set; }

        public Image(string url)
        {
            Source = url;
        }
    }
}
