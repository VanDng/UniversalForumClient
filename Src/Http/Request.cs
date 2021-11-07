using System;

namespace UniversalForumClient.Http
{
    class Request
    {
        public enum Methods
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public string Uri { get; private set; }
        public Methods Method { get; private set; }
        public Request(string uri, Methods method)
        {
            Uri = uri;
            Method = method;
        }
    }
}