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
    public class CardsTest
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
        public async Task TestCreateAndGetGiftCard()
        {
            var program = await _lightrail.Programs.CreateProgram(new CreateProgramParams
            {
                UserSuppliedId = Guid.NewGuid().ToString(),
                Name = ".net cards unit test",
                Currency = "USD",
                CodeMinValue = 1,
                CodeMaxValue = 10000,
                ValueStoreType = ValueStoreType.PRINCIPAL,
                ProgramStartDate = new DateTime(0)
            });
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ProgramId);

            var cardUserSuppliedId = Guid.NewGuid().ToString();
            var card = await _lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = cardUserSuppliedId,
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 6565,
                ProgramId = program.ProgramId
            });
            Assert.IsNotNull(card);
            Assert.IsNotNull(card.CardId);
            Assert.AreEqual(cardUserSuppliedId, card.UserSuppliedId);
            Assert.AreEqual(CardType.GIFT_CARD, card.CardType);
            Assert.AreEqual("USD", card.Currency);

            var cardById = await _lightrail.Cards.GetCardById(card.CardId);
            Assert.IsNotNull(cardById);
            Assert.AreEqual(card.CardId, cardById.CardId);
            Assert.AreEqual(card.UserSuppliedId, cardById.UserSuppliedId);
            Assert.AreEqual(card.CardType, cardById.CardType);
            Assert.AreEqual(card.Currency, cardById.Currency);

            var cardByUserSuppliedId = await _lightrail.Cards.GetCardByUserSuppliedId(cardUserSuppliedId);
            Assert.IsNotNull(cardByUserSuppliedId);
            Assert.AreEqual(card.CardId, cardByUserSuppliedId.CardId);
            Assert.AreEqual(card.UserSuppliedId, cardByUserSuppliedId.UserSuppliedId);
            Assert.AreEqual(card.CardType, cardByUserSuppliedId.CardType);
            Assert.AreEqual(card.Currency, cardByUserSuppliedId.Currency);

            var fullcode = await _lightrail.Cards.GetFullcode(card);
            Assert.IsNotNull(fullcode);
            Assert.IsNotNull(fullcode.Code);

            var details = await _lightrail.Cards.GetDetails(card);
            Assert.IsNotNull(details);
            Assert.AreEqual(card.CardId, details.CardId);
            Assert.AreEqual(card.CardType, details.CardType);
            Assert.AreEqual(1, details.ValueStores.Count);
            Assert.AreEqual(6565, details.ValueStores[0].Value);
        }

        [TestMethod]
        public async Task TestCreateAndActivateGiftCard()
        {
            var program = await _lightrail.Programs.CreateProgram(new CreateProgramParams
            {
                UserSuppliedId = Guid.NewGuid().ToString(),
                Name = ".net cards unit test",
                Currency = "USD",
                CodeMinValue = 1,
                CodeMaxValue = 10000,
                ValueStoreType = ValueStoreType.PRINCIPAL,
                ProgramStartDate = new DateTime(0)
            });
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ProgramId);

            var cardUserSuppliedId = Guid.NewGuid().ToString();
            var card = await _lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = cardUserSuppliedId,
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 7272,
                Inactive = true,
                ProgramId = program.ProgramId
            });
            Assert.IsNotNull(card);
            Assert.IsNotNull(card.CardId);
            Assert.AreEqual(cardUserSuppliedId, card.UserSuppliedId);
            Assert.AreEqual(CardType.GIFT_CARD, card.CardType);
            Assert.AreEqual("USD", card.Currency);

            var transaction = await _lightrail.Cards.ActivateCard(card, Guid.NewGuid().ToString());
            Assert.IsNotNull(transaction);
            Assert.AreEqual(card.CardId, transaction.CardId);
            Assert.AreEqual(card.Currency, transaction.Currency);
        }

        [TestMethod]
        public async Task TestGetGiftCardNotFound()
        {
            var card = await _lightrail.Cards.GetCardById(Guid.NewGuid().ToString());
            Assert.IsNull(card);
        }

        [TestMethod]
        public async Task TestGetGiftCardWithMaliciousPath()
        {
            var exceptionThrown = true;
            try
            {
                await _lightrail.Cards.GetCardById("../..");
            }
            catch (LightrailRequestException e)
            {
                Assert.AreEqual(new Uri(_lightrail.RestRoot, "/v1/cards/..%2F.."), e.RequestUri);
            }
            Assert.IsTrue(exceptionThrown, "throws a LightrailRequestException exception");
        }
    }
}
