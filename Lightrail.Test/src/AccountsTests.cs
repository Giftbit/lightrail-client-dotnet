using Lightrail;
using Lightrail.Model;
using Lightrail.Net.Exceptions;
using Lightrail.Params;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lightrail.Test
{
    [TestClass]
    public class AccountsTest
    {
        private LightrailClient _lightrail;

        [TestInitialize]
        public void Before()
        {
            DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".env"));
            _lightrail = new LightrailClient
            {
                ApiKey = Environment.GetEnvironmentVariable("LIGHTRAIL_API_KEY")
            };
        }

        [TestMethod]
        public async Task TestCreateAndTransactAccount()
        {
            var shopper = new ContactIdentifier { ShopperId = Guid.NewGuid().ToString() };
            var userSuppliedId = Guid.NewGuid().ToString();

            var card = await _lightrail.Accounts.CreateAccount(shopper, new CreateAccountCardParams
            {
                UserSuppliedId = userSuppliedId,
                Currency = "USD",
                InitialValue = 9191
            });
            Assert.IsNotNull(card);
            Assert.IsNotNull(card.CardId);
            Assert.AreEqual(userSuppliedId, card.UserSuppliedId);
            Assert.AreEqual(CardType.ACCOUNT_CARD, card.CardType);
            Assert.AreEqual("USD", card.Currency);

            var cardByGet = await _lightrail.Accounts.GetAccount(shopper, "USD");
            Assert.IsNotNull(cardByGet);
            Assert.AreEqual(card.CardId, cardByGet.CardId);
            Assert.AreEqual(card.UserSuppliedId, cardByGet.UserSuppliedId);
            Assert.AreEqual(card.CardType, cardByGet.CardType);
            Assert.AreEqual(card.Currency, cardByGet.Currency);

            var contact = await _lightrail.Contacts.GetContact(shopper);
            Assert.IsNotNull(contact);
            Assert.AreEqual(card.ContactId, contact.ContactId);

            var pendingTransaction = await _lightrail.Accounts.CreateTransaction(shopper, new CreateTransactionParams
            {
                UserSuppliedId = Guid.NewGuid().ToString(),
                Currency = "USD",
                Value = -5191,
                Pending = true
            });
            Assert.IsNotNull(pendingTransaction);
            Assert.AreEqual(card.CardId, pendingTransaction.CardId);
            Assert.AreEqual(-5191, pendingTransaction.Value);
            Assert.AreEqual(1, pendingTransaction.TransactionBreakdown.Count);
            Assert.AreEqual(-5191, pendingTransaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(4000, pendingTransaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);

            var captureTransaction = await _lightrail.Accounts.CapturePendingTransaction(shopper, pendingTransaction, new CapturePendingTransactionParams
            {
                UserSuppliedId = Guid.NewGuid().ToString()
            });
            Assert.IsNotNull(captureTransaction);
            Assert.AreEqual(card.CardId, captureTransaction.CardId);
            Assert.AreEqual(-5191, captureTransaction.Value);
            Assert.AreEqual(1, captureTransaction.TransactionBreakdown.Count);
            Assert.AreEqual(-5191, captureTransaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(4000, captureTransaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);
        }

        [TestMethod]
        public async Task TestGetNonExistantAccount()
        {
            var shopper = new ContactIdentifier { ShopperId = Guid.NewGuid().ToString() };
            var card = await _lightrail.Accounts.GetAccount(shopper, "AUD");
            Assert.IsNull(card);
        }
    }
}
