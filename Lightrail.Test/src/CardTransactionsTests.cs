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
            Assert.AreEqual(transaction.CardId, card.CardId);
            Assert.AreEqual(transaction.Value, -7474);
            Assert.AreEqual(transaction.TransactionBreakdown.Count, 1);
            Assert.AreEqual(transaction.TransactionBreakdown[0].Value, -7474);
            Assert.AreEqual(transaction.TransactionBreakdown[0].ValueAvailableAfterTransaction, 0);
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
            Assert.AreEqual(transaction.CardId, card.CardId);
            Assert.AreEqual(transaction.Value, -3322);
            Assert.AreEqual(transaction.TransactionType, TransactionType.PENDING_CREATE);
            Assert.AreEqual(transaction.TransactionBreakdown.Count, 1);
            Assert.AreEqual(transaction.TransactionBreakdown[0].Value, -3322);
            Assert.AreEqual(transaction.TransactionBreakdown[0].ValueAvailableAfterTransaction, 119999);

            var captureTransaction = await _lightrail.Cards.Transactions.CapturePending(card, transaction, new CapturePendingTransactionParams { UserSuppliedId = Guid.NewGuid().ToString() });
            Assert.IsNotNull(captureTransaction);
            Assert.AreEqual(captureTransaction.CardId, card.CardId);
            Assert.AreEqual(captureTransaction.Value, -3322);
            Assert.AreEqual(captureTransaction.TransactionType, TransactionType.DRAWDOWN);
            Assert.AreEqual(captureTransaction.TransactionBreakdown.Count, 1);
            Assert.AreEqual(captureTransaction.TransactionBreakdown[0].Value, -3322);
            Assert.AreEqual(captureTransaction.TransactionBreakdown[0].ValueAvailableAfterTransaction, 119999);

            var transactionById = await _lightrail.Cards.Transactions.GetTransactionById(card.CardId, captureTransaction.TransactionId);
            Assert.IsNotNull(transactionById);
            Assert.AreEqual(transactionById.CardId, captureTransaction.CardId);
            Assert.AreEqual(transactionById.Value, captureTransaction.Value);
            Assert.AreEqual(transactionById.TransactionType, captureTransaction.TransactionType);
            Assert.AreEqual(transactionById.TransactionBreakdown.Count, captureTransaction.TransactionBreakdown.Count);
            Assert.AreEqual(transactionById.TransactionBreakdown[0].Value, captureTransaction.TransactionBreakdown[0].Value);
            Assert.AreEqual(transactionById.TransactionBreakdown[0].ValueAvailableAfterTransaction, captureTransaction.TransactionBreakdown[0].ValueAvailableAfterTransaction);
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
            Assert.AreEqual(transaction.CardId, card.CardId);
            Assert.AreEqual(transaction.Value, -3322);
            Assert.AreEqual(transaction.TransactionType, TransactionType.PENDING_CREATE);
            Assert.AreEqual(transaction.TransactionBreakdown.Count, 1);
            Assert.AreEqual(transaction.TransactionBreakdown[0].Value, -3322);
            Assert.AreEqual(transaction.TransactionBreakdown[0].ValueAvailableAfterTransaction, 119999);

            var voidUserSuppliedId = Guid.NewGuid().ToString();
            var voidTransaction = await _lightrail.Cards.Transactions.VoidPending(card, transaction, new VoidPendingTransactionParams { UserSuppliedId = voidUserSuppliedId });
            Assert.IsNotNull(voidTransaction);
            Assert.AreEqual(voidTransaction.CardId, card.CardId);
            Assert.AreEqual(voidTransaction.TransactionType, TransactionType.PENDING_VOID);

            var transactionByUserSuppliedId = await _lightrail.Cards.Transactions.GetTransactionByUserSuppliedId(card, voidUserSuppliedId);
            Assert.IsNotNull(transactionByUserSuppliedId);
            Assert.AreEqual(transactionByUserSuppliedId.CardId, voidTransaction.CardId);
            Assert.AreEqual(transactionByUserSuppliedId.Value, voidTransaction.Value);
            Assert.AreEqual(transactionByUserSuppliedId.TransactionType, voidTransaction.TransactionType);
            Assert.AreEqual(transactionByUserSuppliedId.TransactionBreakdown.Count, voidTransaction.TransactionBreakdown.Count);
        }

        [TestMethod]
        public async Task TestTransactionNotFound()
        {
            var transaction = await _lightrail.Cards.Transactions.GetTransactionById(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.IsNull(transaction);
        }
    }
}
