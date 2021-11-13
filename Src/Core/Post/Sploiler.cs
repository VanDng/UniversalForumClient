using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public class Sploiler
    {
        public object[] Contents { get; private set; }

        public Sploiler(object[] contents)
        {
            Contents = contents;
        }
    }
}
