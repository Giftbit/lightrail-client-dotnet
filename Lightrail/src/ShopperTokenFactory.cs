using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lightrail
{

    public static class ShopperTokenFactory
    {
        
        public static string GenerateShopperToken(string shopperId, int validityInSeconds = 43200)
        {
            if (LightrailConfiguration.ApiKey == null)
            {
                // TODO throw a better Exception type
                throw new Exception("LightrailConfiguration.ApiKey is not set.");
            }
            if (LightrailConfiguration.SharedSecret == null)
            {
                // TODO throw a better Exception type
                throw new Exception("LightrailConfiguration.SharedSecret is not set.");
            }
            if (shopperId == null)
            {
                throw new ArgumentNullException(nameof(shopperId));
            }
            if (validityInSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(validityInSeconds), "must be > 0");
            }

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(LightrailConfiguration.SharedSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(creds)
            {
                {"ver", 3},
                {"vav", 1}
            };

            var nowInSeconds = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var payload = new JwtPayload
            {
                {
                    "g",
                    new Dictionary<string, object>
                    {
                        {"gui", "userid"},
                        {"gmi", "merchantid"},
                        {"coi", "contactid or undefined"},  // TODO decode LightrailConfiguration.ApiKey and use those values
                        {"cui", "Contact usersuppliedid or undefined"},
                        {"shi", "shopperid or undefined"}
                    }
                },
                {"iss", "MERCHANT"},
                {"iat", nowInSeconds},
                {"exp", nowInSeconds + validityInSeconds}
            };

            var secToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }
    }
}
