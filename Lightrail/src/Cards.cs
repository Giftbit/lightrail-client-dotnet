using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lightrail.Model;
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
            request.AddJsonBody(parms);

            // var response = await client.ExecuteTaskAsync<Dictionary<string, Card>>(request);
            // if (response.IsSuccessful)
            // {
            //     return response.Data["card"];
            // }
            // throw response.ErrorException;

            System.Diagnostics.Debug.WriteLine("client.ExecuteTaskAsync");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                System.Diagnostics.Debug.WriteLine("response.Content=", response.Content);
                var j = JObject.Parse(response.Content);
                return ((JObject)j["card"]).ToObject<Card>();
            }

            throw response.ErrorException;
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
