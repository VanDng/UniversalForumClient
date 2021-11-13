using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Undefined
    {
        public string MarkupText { get; private set; }

        public Undefined(string markupText)
        {
            MarkupText = markupText;
        }
    }
}
