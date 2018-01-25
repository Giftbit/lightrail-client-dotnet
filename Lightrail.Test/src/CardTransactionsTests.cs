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
    public class CardTransactionsTest
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
        public async Task TestSimulateTransaction()
        {
            var card = await _lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = Guid.NewGuid().ToString(),
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 7474
            });
            Assert.IsNotNull(card);

            var transaction = await _lightrail.Cards.Transactions.SimulateTransaction(card, new SimulateTransactionParams {
                UserSuppliedId = Guid.NewGuid().ToString(),
                Value = -8000,
                Currency = "USD",
                Nsf = false
            });
            Assert.IsNotNull(transaction);
            Assert.AreEqual(card.CardId, transaction.CardId);
            Assert.AreEqual(-7474, transaction.Value);
            Assert.AreEqual(1, transaction.TransactionBreakdown.Count);
            Assert.AreEqual(-7474, transaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(0, transaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);
        }

        [TestMethod]
        public async Task TestCapturePendingTransaction()
        {
            var card = await _lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = Guid.NewGuid().ToString(),
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 123321
            });
            Assert.IsNotNull(card);

            var transaction = await _lightrail.Cards.Transactions.CreateTransaction(card, new CreateTransactionParams {
                UserSuppliedId = Guid.NewGuid().ToString(),
                Currency = "USD",
                Value = -3322,
                Pending = true
            });
            Assert.IsNotNull(transaction);
            Assert.AreEqual(card.CardId, transaction.CardId);
            Assert.AreEqual(-3322, transaction.Value);
            Assert.AreEqual(TransactionType.PENDING_CREATE, transaction.TransactionType);
            Assert.AreEqual(1, transaction.TransactionBreakdown.Count);
            Assert.AreEqual(-3322, transaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(119999, transaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);

            var captureTransaction = await _lightrail.Cards.Transactions.CapturePending(card, transaction, new CapturePendingTransactionParams { UserSuppliedId = Guid.NewGuid().ToString() });
            Assert.IsNotNull(captureTransaction);
            Assert.AreEqual(card.CardId, captureTransaction.CardId);
            Assert.AreEqual(-3322, captureTransaction.Value);
            Assert.AreEqual(TransactionType.DRAWDOWN, captureTransaction.TransactionType);
            Assert.AreEqual(1, captureTransaction.TransactionBreakdown.Count);
            Assert.AreEqual(-3322, captureTransaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(119999, captureTransaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);

            var transactionById = await _lightrail.Cards.Transactions.GetTransactionById(card.CardId, captureTransaction.TransactionId);
            Assert.IsNotNull(transactionById);
            Assert.AreEqual(captureTransaction.CardId, transactionById.CardId);
            Assert.AreEqual(captureTransaction.Value, transactionById.Value);
            Assert.AreEqual(captureTransaction.TransactionType, transactionById.TransactionType);
            Assert.AreEqual(captureTransaction.TransactionBreakdown.Count, transactionById.TransactionBreakdown.Count);
            Assert.AreEqual(captureTransaction.TransactionBreakdown[0].Value, transactionById.TransactionBreakdown[0].Value);
            Assert.AreEqual(captureTransaction.TransactionBreakdown[0].ValueAvailableAfterTransaction, transactionById.TransactionBreakdown[0].ValueAvailableAfterTransaction);
        }

        [TestMethod]
        public async Task TestVoidPendingTransaction()
        {
            var card = await _lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = Guid.NewGuid().ToString(),
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 123321
            });
            Assert.IsNotNull(card);

            var transaction = await _lightrail.Cards.Transactions.CreateTransaction(card, new CreateTransactionParams {
                UserSuppliedId = Guid.NewGuid().ToString(),
                Currency = "USD",
                Value = -3322,
                Pending = true
            });
            Assert.IsNotNull(transaction);
            Assert.AreEqual(card.CardId, transaction.CardId);
            Assert.AreEqual(-3322, transaction.Value);
            Assert.AreEqual(TransactionType.PENDING_CREATE, transaction.TransactionType);
            Assert.AreEqual(1, transaction.TransactionBreakdown.Count);
            Assert.AreEqual(-3322, transaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(119999, transaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);

            var voidUserSuppliedId = Guid.NewGuid().ToString();
            var voidTransaction = await _lightrail.Cards.Transactions.VoidPending(card, transaction, new VoidPendingTransactionParams { UserSuppliedId = voidUserSuppliedId });
            Assert.IsNotNull(voidTransaction);
            Assert.AreEqual(card.CardId, voidTransaction.CardId);
            Assert.AreEqual(TransactionType.PENDING_VOID, voidTransaction.TransactionType);

            var transactionByUserSuppliedId = await _lightrail.Cards.Transactions.GetTransactionByUserSuppliedId(card, voidUserSuppliedId);
            Assert.IsNotNull(transactionByUserSuppliedId);
            Assert.AreEqual(voidTransaction.CardId, transactionByUserSuppliedId.CardId);
            Assert.AreEqual(voidTransaction.Value, transactionByUserSuppliedId.Value);
            Assert.AreEqual(voidTransaction.TransactionType, transactionByUserSuppliedId.TransactionType);
            Assert.AreEqual(voidTransaction.TransactionBreakdown.Count, transactionByUserSuppliedId.TransactionBreakdown.Count);
        }

        [TestMethod]
        public async Task TestTransactionNotFound()
        {
            var transaction = await _lightrail.Cards.Transactions.GetTransactionById(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.IsNull(transaction);
        }
    }
}
