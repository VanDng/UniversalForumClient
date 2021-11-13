using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Hyperlink
    {
        public string Href { get; private set; }

        public object[] Contents { get; private set; }
        
        public Hyperlink(string href, object[] content)
        {
            Href = href;
            Contents = content;
        }
    }
}
