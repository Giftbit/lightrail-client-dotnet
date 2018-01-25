using Lightrail.Model;
using Lightrail.Params;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lightrail
{
    public class CardTransactions
    {
        private LightrailClient _lightrail;

        internal CardTransactions(LightrailClient lightrail)
        {
            _lightrail = lightrail;
        }

        public async Task<Transaction> CreateTransaction(string cardId, CreateTransactionParams parms)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

            var response = await _lightrail.Request("POST", "v1/cards/{cardId}/transactions")
                .SetPathParameter("cardId", cardId)
                .AddBody(parms)
                .Execute<Dictionary<string, Transaction>>();
            response.EnsureSuccess();
            return response.Body["transaction"];
        }

        public Task<Transaction> CreateTransaction(Card card, CreateTransactionParams parms)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return CreateTransaction(card.CardId, parms);
        }

        public async Task<Transaction> SimulateTransaction(string cardId, SimulateTransactionParams parms)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

            var response = await _lightrail.Request("POST", "v1/cards/{cardId}/transactions/dryRun")
                .SetPathParameter("cardId", cardId)
                .AddBody(parms)
                .Execute<Dictionary<string, Transaction>>();
            response.EnsureSuccess();
            return response.Body["transaction"];
        }

        public Task<Transaction> SimulateTransaction(Card card, SimulateTransactionParams parms)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return SimulateTransaction(card.CardId, parms);
        }

        public async Task<Transaction> GetTransactionById(string cardId, string transactionId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (transactionId == null)
            {
                throw new ArgumentNullException(nameof(transactionId));
            }

            var response = await _lightrail.Request("GET", "v1/cards/{cardId}/transactions/{transactionId}")
                .SetPathParameter("cardId", cardId)
                .SetPathParameter("transactionId", transactionId)
                .Execute<Dictionary<string, Transaction>>();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccess();
            return response.Body["transaction"];
        }

        public Task<Transaction> GetTransactionById(Card card, string transactionId)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            return GetTransactionById(card.CardId, transactionId);
        }

        public async Task<PaginatedTransactions> GetTransactions(string cardId, GetTransactionsParams parms)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var response = await _lightrail.Request("GET", "v1/cards/{cardId}/transactions")
                .SetPathParameter("cardId", cardId)
                .AddQueryParameters(parms)
                .Execute<PaginatedTransactions>();
            response.EnsureSuccess();
            return response.Body;
        }

        public Task<PaginatedTransactions> GetTransactions(Card card, GetTransactionsParams parms)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            
            return GetTransactions(card.CardId, parms);
        }

        public async Task<Transaction> GetTransactionByUserSuppliedId(string cardId, string userSuppliedId)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (userSuppliedId == null)
            {
                throw new ArgumentNullException(nameof(userSuppliedId));
            }

            var response = await GetTransactions(cardId, new GetTransactionsParams { UserSuppliedId = userSuppliedId });
            if (response.Transactions.Count > 0)
            {
                return response.Transactions[0];
            }
            return null;
        }

        public Task<Transaction> GetTransactionByUserSuppliedId(Card card, string userSuppliedId)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            
            return GetTransactionByUserSuppliedId(card.CardId, userSuppliedId);
        }

        public async Task<Transaction> CapturePending(string cardId, string transactionId, CapturePendingTransactionParams parms)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (transactionId == null)
            {
                throw new ArgumentNullException(nameof(transactionId));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

            var response = await _lightrail.Request("POST", "v1/cards/{cardId}/transactions/{transactionId}/capture")
                .SetPathParameter("cardId", cardId)
                .SetPathParameter("transactionId", transactionId)
                .AddBody(parms)
                .Execute<Dictionary<string, Transaction>>();
            response.EnsureSuccess();
            return response.Body["transaction"];
        }

        public Task<Transaction> CapturePending(Card card, string transactionId, CapturePendingTransactionParams parms)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            
            return CapturePending(card.CardId, transactionId, parms);
        }
        
        public Task<Transaction> CapturePending(string cardId, Transaction transaction, CapturePendingTransactionParams parms)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            
            return CapturePending(cardId, transaction.TransactionId, parms);
        }
        
        public Task<Transaction> CapturePending(Card card, Transaction transaction, CapturePendingTransactionParams parms)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            
            return CapturePending(card, transaction.TransactionId, parms);
        }

        public async Task<Transaction> VoidPending(string cardId, string transactionId, VoidPendingTransactionParams parms)
        {
            if (cardId == null)
            {
                throw new ArgumentNullException(nameof(cardId));
            }
            if (transactionId == null)
            {
                throw new ArgumentNullException(nameof(transactionId));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            parms.EnsureUserSuppliedId();

            var response = await _lightrail.Request("POST", "v1/cards/{cardId}/transactions/{transactionId}/void")
                .SetPathParameter("cardId", cardId)
                .SetPathParameter("transactionId", transactionId)
                .AddBody(parms)
                .Execute<Dictionary<string, Transaction>>();
            response.EnsureSuccess();
            return response.Body["transaction"];
        }

        public Task<Transaction> VoidPending(Card card, string transactionId, VoidPendingTransactionParams parms)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }
            
            return VoidPending(card.CardId, transactionId, parms);
        }
        
        public Task<Transaction> VoidPending(string cardId, Transaction transaction, VoidPendingTransactionParams parms)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            
            return VoidPending(cardId, transaction.TransactionId, parms);
        }
        
        public Task<Transaction> VoidPending(Card card, Transaction transaction, VoidPendingTransactionParams parms)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            
            return VoidPending(card, transaction.TransactionId, parms);
        }
    }
}
