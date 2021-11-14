using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class StyleContainer
    {
        public string Style { get; private set; }

        public object[] Contents { get; private set; }

        public StyleContainer(string style, object[] contents)
        {
            Style = style;
            Contents = contents;
        }
    }
}
