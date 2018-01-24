using Lightrail.Model;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lightrail.Test
{
    [TestClass]
    public class LightrailClientTest
    {
        [TestMethod]
        public void TestGenerateShopperTokenForContactId()
        {
            var lightrail = new LightrailClient
            {
                ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJnIjp7Imd1aSI6Imdvb2V5IiwiZ21pIjoiZ2VybWllIn19.XxOjDsluAw5_hdf5scrLk0UBn8VlhT-3zf5ZeIkEld8",
                SharedSecret = "secret"
            };

            var shopperToken = lightrail.GenerateShopperToken(new ContactIdentifier { ContactId = "chauntaktEyeDee" }, 600);
            Assert.IsNotNull(shopperToken);
            Assert.AreEqual(shopperToken.Split(".").Length, 3);

            JwtSecurityToken jwt = ReadToken(shopperToken, lightrail.SharedSecret);
            Assert.AreEqual(jwt.Header.Alg, "HS256");
            Assert.AreEqual(jwt.Header["ver"], (Int64)3);
            Assert.AreEqual(jwt.Header["vav"], (Int64)1);
            Assert.AreEqual(((JObject)jwt.Payload["g"])["gui"], "gooey");
            Assert.AreEqual(((JObject)jwt.Payload["g"])["gmi"], "germie");
            Assert.AreEqual(((JObject)jwt.Payload["g"])["coi"], "chauntaktEyeDee");
            Assert.AreEqual(jwt.Payload.Iss, "MERCHANT");
            Assert.IsNotNull(jwt.Payload.Iat);
            Assert.IsNotNull(jwt.Payload.Exp);
        }

        [TestMethod]
        public void TestGenerateShopperTokenForShopperId()
        {
            var lightrail = new LightrailClient
            {
                ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJnIjp7Imd1aSI6Imdvb2V5IiwiZ21pIjoiZ2VybWllIn19.XxOjDsluAw5_hdf5scrLk0UBn8VlhT-3zf5ZeIkEld8",
                SharedSecret = "secret"
            };

            var shopperToken = lightrail.GenerateShopperToken(new ContactIdentifier { ShopperId = "zhopherId" }, 600);
            Assert.IsNotNull(shopperToken);
            Assert.AreEqual(shopperToken.Split(".").Length, 3);

            JwtSecurityToken jwt = ReadToken(shopperToken, lightrail.SharedSecret);
            Assert.AreEqual(jwt.Header.Alg, "HS256");
            Assert.AreEqual(jwt.Header["ver"], (Int64)3);
            Assert.AreEqual(jwt.Header["vav"], (Int64)1);
            Assert.AreEqual(((JObject)jwt.Payload["g"])["gui"], "gooey");
            Assert.AreEqual(((JObject)jwt.Payload["g"])["gmi"], "germie");
            Assert.AreEqual(((JObject)jwt.Payload["g"])["shi"], "zhopherId");
            Assert.AreEqual(jwt.Payload.Iss, "MERCHANT");
            Assert.IsNotNull(jwt.Payload.Iat);
            Assert.IsNotNull(jwt.Payload.Exp);
        }

        [TestMethod]
        public void TestGenerateShopperTokenForUserSuppliedId()
        {
            var lightrail = new LightrailClient
            {
                ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJnIjp7Imd1aSI6Imdvb2V5IiwiZ21pIjoiZ2VybWllIn19.XxOjDsluAw5_hdf5scrLk0UBn8VlhT-3zf5ZeIkEld8",
                SharedSecret = "secret"
            };

            var shopperToken = lightrail.GenerateShopperToken(new ContactIdentifier { UserSuppliedId = "luserSuppliedId" }, 600);
            Assert.IsNotNull(shopperToken);
            Assert.AreEqual(shopperToken.Split(".").Length, 3);

            JwtSecurityToken jwt = ReadToken(shopperToken, lightrail.SharedSecret);
            Assert.AreEqual(jwt.Header.Alg, "HS256");
            Assert.AreEqual(jwt.Header["ver"], (Int64)3);
            Assert.AreEqual(jwt.Header["vav"], (Int64)1);
            Assert.AreEqual(((JObject)jwt.Payload["g"])["gui"], "gooey");
            Assert.AreEqual(((JObject)jwt.Payload["g"])["gmi"], "germie");
            Assert.AreEqual(((JObject)jwt.Payload["g"])["cui"], "luserSuppliedId");
            Assert.AreEqual(jwt.Payload.Iss, "MERCHANT");
            Assert.IsNotNull(jwt.Payload.Iat);
            Assert.IsNotNull(jwt.Payload.Exp);
        }

        private JwtSecurityToken ReadToken(string shopperToken, string sharedSecret)
        {
            var keyBytes = Encoding.Default.GetBytes(sharedSecret);
            if (keyBytes.Length < 64)
            {
                Array.Resize(ref keyBytes, 64);
            }
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            SecurityToken token;
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(shopperToken,
                        new TokenValidationParameters()
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            IssuerSigningKey = key
                        }, out token);

            return (JwtSecurityToken) token;
        }
    }
}
