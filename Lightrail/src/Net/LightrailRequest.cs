using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lightrail.Net
{
    internal class LightrailRequest
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new [] {new StringEnumConverter()}
        };

        private HttpClient _httpClient;
        private HttpMethod _method;
        private Uri _requestUri;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
        private string _body;

        public LightrailRequest(HttpClient httpClient, HttpMethod method, Uri requestUri)
        {
            _httpClient = httpClient;
            _method = method;
            _requestUri = requestUri;
        }

        public LightrailRequest AddHeader(string key, string value)
        {
            _headers.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public LightrailRequest AddHeader(KeyValuePair<string, string> kvp)
        {
            _headers.Add(kvp);
            return this;
        }

        public LightrailRequest AddHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            _headers.AddRange(headers);
            return this;
        }

        public LightrailRequest AddBody(object obj)
        {
            _body = JsonConvert.SerializeObject(obj, JsonSerializerSettings);
            return this;
        }

        public LightrailRequest SetPathParameter(string pathParam, string value)
        {
            _requestUri = new Uri(_requestUri.ToString().Replace("{" + pathParam + "}", Uri.EscapeDataString(value)), UriKind.Absolute);
            return this;
        }

        public LightrailRequest AddQueryParameter(string key, string value)
        {
            var req = _requestUri.ToString();
            var segment = $"{Uri.EscapeDataString(key)}=Uri.EscapeDataString(value)";
            if (req.Contains("?"))
            {
                req = $"{req}&{segment}";
            }
            else
            {
                req = $"{req}?{segment}";
            }
            _requestUri = new Uri(req, UriKind.Absolute);
            return this;
        }

        public LightrailRequest AddQueryParameters(IDictionary<string, string> parameters)
        {
            var req = _requestUri.ToString();
            foreach (var parameter in parameters)
            {
                var segment = $"{Uri.EscapeDataString(parameter.Key)}=Uri.EscapeDataString(parameter.Value)";
                if (req.Contains("?"))
                {
                    req = $"{req}&{segment}";
                }
                else
                {
                    req = $"{req}?{segment}";
                }
            }
            _requestUri = new Uri(req, UriKind.Absolute);
            return this;
        }

        public async Task<LightrailResponse<T>> Execute<T>()
        {
            var reqMessage = new HttpRequestMessage(_method, _requestUri);
            if (_body != null)
            {
                reqMessage.Content = new StringContent(_body);
            }
            foreach (var kvp in _headers)
            {
                reqMessage.Headers.Add(kvp.Key, kvp.Value);
            }

            var respMessage = await _httpClient.SendAsync(reqMessage);
            return new LightrailResponse<T>
            {
                Method = _method,
                RequestUri = _requestUri,
                StatusCode = respMessage.StatusCode,
                BodyText = await respMessage.Content.ReadAsStringAsync()
            };
        }
    }
}
