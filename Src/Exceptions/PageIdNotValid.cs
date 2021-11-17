using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Exceptions
{
    public interface IUniversalForumClientException
    {
        public string Message { get; }
    }

    public abstract class PageIdNotValid : Exception
    {
        public PageIdNotValid(string message)
         : base()
        {

        }
    }

    public class ThreadIdNotValid : PageIdNotValid, IUniversalForumClientException
    {
        private const string _Message = "Thread ID can not be empty";

        public ThreadIdNotValid()
            : base (_Message)
        { }

        string IUniversalForumClientException.Message
        { 
            get => _Message;
        }
    }
}
