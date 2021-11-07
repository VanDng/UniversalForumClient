using System;
using System.Net.Http;

namespace UniversalForumClient.Http
{
    class RequestDetail
    {
        public DateTime LastTimeRequest { get; set; }
        public bool IsWaitingForResponse { get; set; }
        public HttpResponseMessage LastResponse { get; set; }
    }
}