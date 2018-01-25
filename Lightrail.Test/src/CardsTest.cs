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
            var userSuppliedId = Guid.NewGuid().ToString();

            var card = await _lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = userSuppliedId,
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 6565
            });
            Assert.IsNotNull(card);
            Assert.IsNotNull(card.CardId);
            Assert.AreEqual(card.UserSuppliedId, userSuppliedId);
            Assert.AreEqual(card.CardType, CardType.GIFT_CARD);
            Assert.AreEqual(card.Currency, "USD");

            var cardById = await _lightrail.Cards.GetCardById(card.CardId);
            Assert.IsNotNull(cardById);
            Assert.AreEqual(cardById.CardId, card.CardId);
            Assert.AreEqual(cardById.UserSuppliedId, card.UserSuppliedId);
            Assert.AreEqual(cardById.CardType, card.CardType);
            Assert.AreEqual(cardById.Currency, card.Currency);

            var cardByUserSuppliedId = await _lightrail.Cards.GetCardByUserSuppliedId(userSuppliedId);
            Assert.IsNotNull(cardByUserSuppliedId);
            Assert.AreEqual(cardByUserSuppliedId.CardId, card.CardId);
            Assert.AreEqual(cardByUserSuppliedId.UserSuppliedId, card.UserSuppliedId);
            Assert.AreEqual(cardByUserSuppliedId.CardType, card.CardType);
            Assert.AreEqual(cardByUserSuppliedId.Currency, card.Currency);
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
                Assert.AreEqual(e.RequestUri, new Uri(_lightrail.RestRoot, "/v1/cards/..%2F.."));
            }
            Assert.IsTrue(exceptionThrown, "throws a LightrailRequestException exception");
        }
    }
}
