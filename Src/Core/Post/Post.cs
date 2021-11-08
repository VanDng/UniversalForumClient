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

        public ContentManager Content { get; private set; }

        public Post(string author)
        {
            Author = author;
        }
    }
}
