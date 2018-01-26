using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lightrail.Net
{
    /// <summary>
    /// A request to the Lightrail server.  Execute with <see cref="LightrailRequest.Execute()"/>.
    /// </summary>
    public class LightrailRequest
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
        private ILogger _logger;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
        private string _body;

        public LightrailRequest(HttpClient httpClient, HttpMethod method, Uri requestUri, ILogger logger = null)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (requestUri == null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            _httpClient = httpClient;
            _method = method;
            _requestUri = requestUri;
            _logger = logger;
        }

        public HttpMethod Method => _method;
        public Uri RequestUri => _requestUri;

        public LightrailRequest AddHeader(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

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
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            _headers.AddRange(headers);
            return this;
        }

        public LightrailRequest AddBody(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _body = JsonConvert.SerializeObject(obj, JsonSerializerSettings);
            return this;
        }

        public LightrailRequest SetPathParameter(string pathParam, string value)
        {
            if (pathParam == null)
            {
                throw new ArgumentNullException(nameof(pathParam));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _requestUri = new Uri(_requestUri.ToString().Replace("{" + pathParam + "}", Uri.EscapeDataString(value)), UriKind.Absolute);
            return this;
        }

        public LightrailRequest AddQueryParameter(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return AddQueryParameters(new Dictionary<string, string>() {{key, value}});
        }

        public LightrailRequest AddQueryParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var req = _requestUri.ToString();
            foreach (var parameter in parameters)
            {
                var segment = $"{Uri.EscapeDataString(parameter.Key)}={Uri.EscapeDataString(parameter.Value)}";
                if (!req.Contains("?"))
                {
                    req = $"{req}?{segment}";
                }
                else
                {
                    req = $"{req}&{segment}";
                }
            }
            _requestUri = new Uri(req, UriKind.Absolute);
            return this;
        }

        public LightrailRequest AddQueryParameters(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            return AddQueryParameters(JObject.FromObject(o, JsonSerializer.Create(JsonSerializerSettings)).ToObject<Dictionary<string, string>>());
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

            var stopwatch = Stopwatch.StartNew();
            var respMessage = await _httpClient.SendAsync(reqMessage);
            stopwatch.Stop();
            _logger?.LogInformation("{0,-6} {1} {2} ({3}ms)", new object[] {_method, (int)respMessage.StatusCode, _requestUri, stopwatch.ElapsedMilliseconds});
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
