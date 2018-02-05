using Lightrail.Model;
using Lightrail.Net.Exceptions;
using Lightrail.Params;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lightrail
{
    public class Accounts
    {
        private LightrailClient _lightrail;

        internal Accounts(LightrailClient lightrail)
        {
            _lightrail = lightrail;
        }

        /// <summary>
        /// Creates a contact first if contact doesn't exist (if userSuppliedId or shopperId provided)
        /// but throws error if contactId provided and contact not found (can't create a contact 'by contactId').
        /// </summary>
        public async Task<Card> CreateAccount(ContactIdentifier ci, CreateAccountCardParams parms)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            if (parms.Currency == null)
            {
                throw new ArgumentException("parms.Currency is required");
            }
            parms.EnsureUserSuppliedId();

            var contactId = await GetContactId(ci);
            if (contactId == null)
            {
                if (ci.UserSuppliedId != null || ci.ShopperId != null)
                {
                    var contact = await _lightrail.Contacts.CreateContact(new CreateContactParams { UserSuppliedId = ci.UserSuppliedId ?? ci.ShopperId });
                    contactId = contact.ContactId;
                }
                else
                {
                    throw new LightrailRequestException($"could not find contact with contactId {ci.ContactId}") { Status = 404 };
                }
            }

            var card = await GetCard(contactId, parms.Currency);
            if (card == null)
            {
                card = await _lightrail.Cards.CreateCard(new CreateCardParams
                {
                    UserSuppliedId = parms.UserSuppliedId,
                    ContactId = contactId,
                    Currency = parms.Currency,
                    InitialValue = parms.InitialValue,
                    Categories = parms.Categories,
                    Expires = parms.Expires,
                    StartDate = parms.StartDate,
                    Inactive = parms.Inactive,
                    Metadata = parms.Metadata,
                    ProgramId = parms.ProgramId,
                    CardType = CardType.ACCOUNT_CARD
                });
            }

            return card;
        }

        /// <summary>
        /// Get a Card by ContactIdentifier and currency.  Returns null if it doesn't not exist.
        /// </summary>
        public async Task<Card> GetAccount(ContactIdentifier ci, string currency)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            var contactId = await GetContactId(ci);
            if (contactId == null)
            {
                return null;
            }

            return await GetCard(contactId, currency);
        }

        public async Task<Transaction> CreateTransaction(ContactIdentifier ci, CreateTransactionParams parms)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            if (parms.Currency == null)
            {
                throw new ArgumentException("parms.Currency is required");
            }

            var contactId = await GetContactId(ci);
            if (contactId == null)
            {
                throw new LightrailRequestException("could not find contact to transaction against") { Status = 404 };
            }

            var card = await GetCard(contactId, parms.Currency);
            if (card == null)
            {
                throw new LightrailRequestException("could not find card to transaction against") { Status = 404 };
            }

            return await _lightrail.Cards.Transactions.CreateTransaction(card, parms);
        }

        public async Task<Transaction> CapturePendingTransaction(ContactIdentifier ci, Transaction transaction, CapturePendingTransactionParams parms)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var contactId = await GetContactId(ci);
            if (contactId == null)
            {
                throw new LightrailRequestException("could not find contact to transaction against") { Status = 404 };
            }

            var card = await GetCard(contactId, transaction.Currency);
            if (card == null)
            {
                throw new LightrailRequestException("could not find card to transaction against") { Status = 404 };
            }

            return await _lightrail.Cards.Transactions.CapturePending(card, transaction, parms);
        }

        public async Task<Transaction> VoidPendingTransaction(ContactIdentifier ci, Transaction transaction, VoidPendingTransactionParams parms)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }

            var contactId = await GetContactId(ci);
            if (contactId == null)
            {
                throw new LightrailRequestException("could not find contact to transaction against") { Status = 404 };
            }

            var card = await GetCard(contactId, transaction.Currency);
            if (card == null)
            {
                throw new LightrailRequestException("could not find card to transaction against") { Status = 404 };
            }

            return await _lightrail.Cards.Transactions.VoidPending(card, transaction, parms);
        }

        public async Task<Transaction> SimulateTransaction(ContactIdentifier ci, SimulateTransactionParams parms)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }
            if (parms == null)
            {
                throw new ArgumentNullException(nameof(parms));
            }
            if (parms.Currency == null)
            {
                throw new ArgumentException("SimulateTransactionParams.Currency is required");
            }

            var contactId = await GetContactId(ci);
            if (contactId == null)
            {
                throw new LightrailRequestException("could not find contact to simulate transaction against") { Status = 404 };
            }

            var card = await GetCard(contactId, parms.Currency);
            if (card == null)
            {
                throw new LightrailRequestException("could not find card to simulate transaction against") { Status = 404 };
            }

            return await _lightrail.Cards.Transactions.SimulateTransaction(card, parms);
        }

        private async Task<string> GetContactId(ContactIdentifier ci)
        {
            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }

            if (ci.ContactId != null)
            {
                return ci.ContactId;
            }
            var contact = await _lightrail.Contacts.GetContact(ci);
            return contact?.ContactId;
        }

        private async Task<Card> GetCard(string contactId, string currency)
        {
            if (contactId == null)
            {
                throw new ArgumentNullException(nameof(contactId));
            }
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            var cardsResp = await _lightrail.Cards.GetCards(new GetCardsParams { ContactId = contactId, Currency = currency });
            return cardsResp.Cards.Count > 0 ? cardsResp.Cards[0] : null;
        }
    }
}
