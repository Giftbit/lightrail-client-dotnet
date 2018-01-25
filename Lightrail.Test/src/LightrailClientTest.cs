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
            Assert.AreEqual(3, shopperToken.Split(".").Length);

            JwtSecurityToken jwt = ReadToken(shopperToken, lightrail.SharedSecret);
            Assert.AreEqual("HS256", jwt.Header.Alg);
            Assert.AreEqual((Int64)3, jwt.Header["ver"]);
            Assert.AreEqual((Int64)1, jwt.Header["vav"]);
            Assert.AreEqual("gooey", ((JObject)jwt.Payload["g"])["gui"]);
            Assert.AreEqual("germie", ((JObject)jwt.Payload["g"])["gmi"]);
            Assert.AreEqual("chauntaktEyeDee", ((JObject)jwt.Payload["g"])["coi"]);
            Assert.AreEqual("MERCHANT", jwt.Payload.Iss);
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
            Assert.AreEqual(3, shopperToken.Split(".").Length);

            JwtSecurityToken jwt = ReadToken(shopperToken, lightrail.SharedSecret);
            Assert.AreEqual("HS256", jwt.Header.Alg);
            Assert.AreEqual((Int64)3, jwt.Header["ver"]);
            Assert.AreEqual((Int64)1, jwt.Header["vav"]);
            Assert.AreEqual("gooey", ((JObject)jwt.Payload["g"])["gui"]);
            Assert.AreEqual("germie", ((JObject)jwt.Payload["g"])["gmi"]);
            Assert.AreEqual("zhopherId", ((JObject)jwt.Payload["g"])["shi"]);
            Assert.AreEqual("MERCHANT", jwt.Payload.Iss);
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
            Assert.AreEqual(3, shopperToken.Split(".").Length);

            JwtSecurityToken jwt = ReadToken(shopperToken, lightrail.SharedSecret);
            Assert.AreEqual("HS256", jwt.Header.Alg);
            Assert.AreEqual((Int64)3, jwt.Header["ver"]);
            Assert.AreEqual((Int64)1, jwt.Header["vav"]);
            Assert.AreEqual("gooey", ((JObject)jwt.Payload["g"])["gui"]);
            Assert.AreEqual("germie", ((JObject)jwt.Payload["g"])["gmi"]);
            Assert.AreEqual("luserSuppliedId", ((JObject)jwt.Payload["g"])["cui"]);
            Assert.AreEqual("MERCHANT", jwt.Payload.Iss);
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
