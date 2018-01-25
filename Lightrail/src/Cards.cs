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
        private CardTransactions _cardTransactions;

        internal Cards(LightrailClient lightrail)
        {
            _lightrail = lightrail;
        }

        public CardTransactions Transactions => _cardTransactions != null ? _cardTransactions : _cardTransactions = new CardTransactions(_lightrail);

        public async Task<Card> CreateCard(CreateCardParams parms)
        {
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

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
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
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

        public async Task<Fullcode> GetFullcode(string cardId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }

            var response = await _lightrail.Request("GET", "v1/cards/{cardId}/fullcode")
                .SetPathParameter("cardId", cardId)
                .Execute<Dictionary<string, Fullcode>>();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccess();
            return response.Body["fullcode"];
        }

        public Task<Fullcode> GetFullcode(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return GetFullcode(card.CardId);
        }

        public async Task<CardDetails> GetDetails(string cardId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }

            var response = await _lightrail.Request("GET", "v1/cards/{cardId}/details")
                .SetPathParameter("cardId", cardId)
                .Execute<Dictionary<string, CardDetails>>();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccess();
            return response.Body["details"];
        }

        public Task<CardDetails> GetDetails(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return GetDetails(card.CardId);
        }

        public async Task<Card> CancelCard(string cardId, string userSuppliedId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (userSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(userSuppliedId));
            }

            var response = await _lightrail.Request("POST", "v1/cards/{cardId}/cancel")
                .SetPathParameter("cardId", cardId)
                .AddBody(new {UserSuppliedId = userSuppliedId})
                .Execute<Dictionary<string, Card>>();
            response.EnsureSuccess();
            return response.Body["card"];
        }

        public Task<Card> CancelCard(Card card, string userSuppliedId)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return CancelCard(card.CardId, userSuppliedId);
        }

        public async Task<Transaction> ActivateCard(string cardId, string userSuppliedId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (userSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(userSuppliedId));
            }

            var response = await _lightrail.Request("POST", "v1/cards/{cardId}/activate")
                .SetPathParameter("cardId", cardId)
                .AddBody(new {UserSuppliedId = userSuppliedId})
                .Execute<Dictionary<string, Transaction>>();
            response.EnsureSuccess();
            return response.Body["transaction"];
        }

        public Task<Transaction> ActivateCard(Card card, string userSuppliedId)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return ActivateCard(card.CardId, userSuppliedId);
        }
    }
}
