using Lightrail.Model;
using Lightrail.Net.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;

namespace Lightrail.Net
{
    internal class LightrailResponse<T>
    {

        private JObject _bodyJson;
        private T _body;

        public string BodyText { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Uri RequestUri { get; set; }
        public HttpMethod Method { get; set; }

        public bool IsSuccess
        {
            get { return (int)StatusCode >= 200 && (int)StatusCode <= 299; }
        }

        public JObject BodyJson {
            get
            {
                if (_bodyJson == null)
                {
                    _bodyJson = JObject.Parse(BodyText);
                }
                return _bodyJson;
            }
        }

        public T Body {
            get
            {
                if (_body == null)
                {
                    _body = BodyJson.ToObject<T>();
                }
                return _body;
            }
        }

        public void EnsureSuccess()
        {
            if (!IsSuccess)
            {
                try
                {
                    var error = BodyJson.ToObject<Error>();
                    throw new LightrailRequestException(error.Message != null ? error.Message : BodyText)
                    {
                        Status = error.Status != 0 ? error.Status : (int)StatusCode,
                        MessageCode = error.MessageCode,
                        Method = Method,
                        RequestUri = RequestUri
                    };
                }
                catch (JsonReaderException)
                {
                    throw new LightrailRequestException(BodyText)
                    {
                        Status = (int)StatusCode,
                        Method = Method,
                        RequestUri = RequestUri
                    };
                }
            }
        }
    }
}