using System;
using System.Net;

namespace DictaFoule.Mobile.iOS.Business
{
    public class RequestException : Exception
    {
        public HttpStatusCode Code {get; }
        public string Reason { get; }

        public RequestException(HttpStatusCode code, string reason, string message) : base(message)
        {
            this.Code = code;
            this.Reason = reason;
        }
    }
}
