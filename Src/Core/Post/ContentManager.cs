using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class ContentManager
    {
        private List<BaseContent> _contents;

        public ContentManager()
        {
            _contents = new List<BaseContent>();
        }
    }
}
