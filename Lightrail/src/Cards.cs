using Lightrail.Model;
using Lightrail.Params;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lightrail
{
    public class Cards
    {
        private LightrailClient _lightrail;

        internal Cards(LightrailClient lightrail)
        {
            _lightrail = lightrail;
        }

        public async Task<Card> CreateCard(CreateCardParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            if (parms.UserSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(parms.UserSuppliedId));
            }

            var response = await _lightrail.Request("POST", "v1/cards")
                .AddBody(parms)
                .Execute<Dictionary<string, Card>>();
            response.EnsureSuccess();
            return response.Body["card"];
        }

        public async Task<PaginatedCards> GetCards(GetCardsParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var response = await _lightrail.Request("GET", "v1/cards")
                .AddQueryParameters(parms)
                .Execute<PaginatedCards>();
            response.EnsureSuccess();
            return response.Body;
        }

        public async Task<Card> GetCardById(string cardId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }

            var response = await _lightrail.Request("GET", "v1/cards/{cardId}")
                .SetPathParameter("cardId", cardId)
                .Execute<Dictionary<string, Card>>();
            response.EnsureSuccess();
            return response.Body["card"];
        }

        public async Task<Card> GetCardByUserSuppliedId(string userSuppliedId)
        {
            if (userSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(userSuppliedId));
            }

            var resp = await GetCards(new GetCardsParams { UserSuppliedId = userSuppliedId });
            if (resp.Cards.Count > 0)
            {
                return resp.Cards[0];
            }
            return null;
        }
    }
}
