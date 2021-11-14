using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Quote
    {
        public string Author { get; private set; }

        public object[] Contents { get; private set; }

        public Quote(string author, IEnumerable<object> contents)
        {
            Author = author;
            Contents = contents.ToArray();
        }
    }
}
