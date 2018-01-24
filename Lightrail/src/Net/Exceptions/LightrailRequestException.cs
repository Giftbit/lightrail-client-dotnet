using System;
using System.Net;
using System.Net.Http;

namespace Lightrail.Net.Exceptions
{
    public class LightrailRequestException : Exception
    {
        public LightrailRequestException()
        {
        }

        public LightrailRequestException(string message) : base(message)
        {
        }

        public LightrailRequestException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected LightrailRequestException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            
        }

        public int Status { get; internal set; }
        public string MessageCode { get; internal set; }
        public HttpMethod Method { get; internal set; }
        public Uri RequestUri { get; internal set; }
    }
}
