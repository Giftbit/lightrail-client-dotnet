using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lightrail.Model;
using Lightrail.Net;
using Lightrail.Params;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lightrail
{
    static public class Cards
    {
        public static async Task<Card> CreateCard(CreateCardParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            if (parms.UserSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(parms.UserSuppliedId));
            }

            var client = new RestClient(LightrailConfiguration.RestRoot);
            client.Authenticator = new JwtAuthenticator(LightrailConfiguration.ApiKey);

            var request = new RestRequest("v1/cards", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.JsonSerializer = new NewtonSoftJsonSerializer();
            request.AddJsonBody(parms);

            Console.WriteLine("Body=" + request.JsonSerializer.Serialize(parms));

            var response = await client.ExecuteTaskAsync<Dictionary<string, Card>>(request);
            if (response.IsSuccessful)
            {
                return response.Data["card"];
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            // Console.WriteLine("client.ExecuteTaskAsync");
            // var response = await client.ExecuteTaskAsync(request);
            // if (response.IsSuccessful)
            // {
            //     Console.WriteLine("response.Content=" + response.Content);
            //     var j = JObject.Parse(response.Content);
            //     return ((JObject)j["card"]).ToObject<Card>();
            // }

            Console.WriteLine("LightrailConfiguration.RestRoot=" + LightrailConfiguration.RestRoot);
            Console.WriteLine("StatusCode=" + response.StatusCode);
            Console.WriteLine("ResponseStatus=" + response.ResponseStatus);
            Console.WriteLine("ErrorException=" + response.ErrorException);
            Console.WriteLine("ErrorMessage=" + response.ErrorMessage);
            Console.WriteLine("Content=" + response.Content);
            Console.WriteLine("ContentEncoding=" + response.ContentEncoding);
            Console.WriteLine("ContentType=" + response.ContentType);
            Console.WriteLine("Headers=" + string.Join(",", response.Headers));
            Console.WriteLine("Server=" + response.Server);
            Console.WriteLine("ResponseUri=" + response.ResponseUri);

            throw new NotImplementedException();   // TODO throw a LightrailException
        }

        public static async Task<Card> GetCardById(string cardId)
        {
            var client = new RestClient(LightrailConfiguration.RestRoot);
            client.Authenticator = new JwtAuthenticator(LightrailConfiguration.ApiKey);

            var request = new RestRequest("v1/cards/{cardId}", Method.GET);
            request.AddUrlSegment("cardId", cardId);

            var response = await client.ExecuteTaskAsync<Dictionary<string, Card>>(request);
            if (response.IsSuccessful)
            {
                return response.Data["card"];
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            throw response.ErrorException;
        }
    }
}
