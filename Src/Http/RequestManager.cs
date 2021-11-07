using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace UniversalForumClient.Http
{
    /*
        Manage requests and last time the request was made
    */
    class RequestManager
    {
        private ConcurrentDictionary<Request, RequestDetail> _requests;

        public int MaxTimeRequest { get; private set; }

        public RequestManager()
        {
            _requests = new ConcurrentDictionary<Request, RequestDetail>();
            MaxTimeRequest = 5000;
        }
        
        public bool IsRequestAllowed(Request request)
        {
            if (_requests.ContainsKey(request))
            {
                if (DateTime.Now.Subtract(_requests[request].LastTimeRequest).TotalSeconds < MaxTimeRequest)
                {
                    return false;
                }                
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public void AddRequest(Request request)
        {
            _requests.TryAdd(request, new RequestDetail());
        }

        public bool Contains(Request request)
        {
            return _requests.ContainsKey(request);
        }

        public void UpdateRequestDetail(Request request, bool IsWaitingForResponse)
        {
            if (_requests.ContainsKey(request))
            {
                _requests[request].IsWaitingForResponse = IsWaitingForResponse;
            }
        }

        public void UpdateRequestDetail(Request request, HttpResponseMessage httpResponseMessage)
        {
            var detail = _requests[request];
            detail.LastTimeRequest = DateTime.UtcNow;
            detail.LastResponse = httpResponseMessage;
        }

        public HttpResponseMessage GetLastResponse(Request request)
        {
            return _requests[request].LastResponse;
        }
    }
}

