using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Post
    {
        public string Author { get; private set; }

        public object[] Contents { get; private set; }

        public Post(string author, IEnumerable<object> contents)
        {
            Author = author;
            Contents = contents.ToArray();
        }
    }
}
