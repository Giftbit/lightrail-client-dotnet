using Lightrail.Model;
using Lightrail.Net;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lightrail
{
    public class LightrailClient
    {
        private IList<KeyValuePair<string, string>> _additionalHeaders = new List<KeyValuePair<string, string>>();
        private HttpClient _httpClient = new HttpClient();
        private string _userAgent = $"Lightrail-Dotnet/{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        private Cards _cards;

        public string ApiKey { get; set; }
        public string SharedSecret { get; set; }
        public Uri RestRoot { get; set; } = new Uri("https://api.lightrail.com");
        public IList<KeyValuePair<string, string>> AdditionalHeaders => _additionalHeaders;
        public Cards Cards => _cards != null ? _cards : _cards = new Cards(this);

        public LightrailRequest Request(string method, string path)
        {
            return Request(new HttpMethod(method), path);
        }

        public LightrailRequest Request(HttpMethod method, string path)
        {
            Uri requestUri = new Uri(RestRoot, path);

            return new LightrailRequest(_httpClient, method, requestUri)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", $"Bearer {ApiKey}")
                .AddHeader("User-Agent", _userAgent)
                .AddHeaders(_additionalHeaders);
        }

        public string GenerateShopperToken(ContactIdentifier contact, int validityInSeconds = 43200)
        {
            if (ApiKey == null)
            {
                throw new InvalidOperationException("ApiKey is not set.");
            }
            if (SharedSecret == null)
            {
                throw new InvalidOperationException("SharedSecret is not set.");
            }
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            if (contact.ContactId == null && contact.ShopperId == null && contact.UserSuppliedId == null)
            {
                throw new ArgumentException(nameof(contact), "one of ContactId, ShopperId or UserSupliedId must be set");
            }
            if (validityInSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(validityInSeconds), "must be > 0");
            }

            var handler = new JwtSecurityTokenHandler();
            var merchantToken = (JwtSecurityToken) handler.ReadToken(ApiKey);
            if (merchantToken.Payload["g"] == null || ((JObject)merchantToken.Payload["g"])["gui"] == null)
            {
                throw new InvalidOperationException("LightrailConfiguration.ApiKey is not valid.");
            }

            var keyBytes = Encoding.Default.GetBytes(SharedSecret);
            if (keyBytes.Length < 64)
            {
                Array.Resize(ref keyBytes, 64);
            }
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(creds)
            {
                {"ver", 3},
                {"vav", 1}
            };

            var g = new Dictionary<string, object>
            {
                {"gui", ((JObject)merchantToken.Payload["g"])["gui"]},
                {"gmi", ((JObject)merchantToken.Payload["g"])["gmi"]}
            };
            if (contact.ContactId != null) {
                g["coi"] = contact.ContactId;
            }
            if (contact.ShopperId != null) {
                g["shi"] = contact.ShopperId;
            }
            if (contact.UserSuppliedId != null) {
                g["cui"] = contact.UserSuppliedId;
            }
            var nowInSeconds = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var payload = new JwtPayload
            {
                {"g", g},
                {"iss", "MERCHANT"},
                {"iat", nowInSeconds},
                {"exp", nowInSeconds + validityInSeconds}
            };

            var secToken = new JwtSecurityToken(header, payload);
            return handler.WriteToken(secToken);
        }
    }
}
