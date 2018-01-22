using Lightrail.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lightrail
{

    public static class ShopperTokenFactory
    {
        
        public static string GenerateShopperToken(ContactIdentifier contact, int validityInSeconds = 43200)
        {
            if (LightrailConfiguration.ApiKey == null)
            {
                throw new InvalidOperationException("LightrailConfiguration.ApiKey is not set.");
            }
            if (LightrailConfiguration.SharedSecret == null)
            {
                throw new InvalidOperationException("LightrailConfiguration.SharedSecret is not set.");
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
            var merchantToken = (JwtSecurityToken) handler.ReadToken(LightrailConfiguration.ApiKey);
            if (merchantToken.Payload["g"] == null || ((JObject)merchantToken.Payload["g"])["gui"] == null)
            {
                throw new InvalidOperationException("LightrailConfiguration.ApiKey is not valid.");
            }

            var keyBytes = Encoding.Default.GetBytes(LightrailConfiguration.SharedSecret);
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
