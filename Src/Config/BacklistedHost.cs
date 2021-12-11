using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Config
{
    public static class BacklistedHost
    {
        public static IEnumerable<string> Items()
        {
            return new string[]
            {
                "l.yimg.com"
            };
        }
    }
}
