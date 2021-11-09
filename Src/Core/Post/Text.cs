using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Text
    {
        public string PlainText { get; private set; }
        public string MarkupText { get; private set; }

        public Text(string plainText, string markupText)
        {
            PlainText = plainText;
            MarkupText = markupText;
        }
    }
}
