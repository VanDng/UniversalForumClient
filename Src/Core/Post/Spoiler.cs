using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Spoiler
    {
        public object[] Contents { get; private set; }

        public Spoiler(object[] contents)
        {
            Contents = contents;
        }
    }
}
