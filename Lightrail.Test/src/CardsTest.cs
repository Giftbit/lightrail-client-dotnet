using Lightrail;
using Lightrail.Model;
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
        [TestInitialize()]
        public void Before()
        {
            DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".env"));
            LightrailConfiguration.ApiKey = Environment.GetEnvironmentVariable("LIGHTRAIL_API_KEY");
        }

        [TestMethod]
        public async Task TestCreateGiftCardCard()
        {
            var userSuppliedId = Guid.NewGuid().ToString();

            var card = await Lightrail.Cards.CreateCard(new CreateCardParams
            {
                UserSuppliedId = userSuppliedId,
                CardType = CardType.GIFT_CARD,
                Currency = "USD",
                InitialValue = 6565
            });

            Assert.IsNotNull(card);
            Assert.AreEqual(card.UserSuppliedId, userSuppliedId);
            Assert.AreEqual(card.CardType, CardType.GIFT_CARD);
            Assert.AreEqual(card.Currency, "USD");
        }
    }
}
