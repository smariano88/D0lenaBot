using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.App.Infrastructure.Telegram;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.UnitTest.App.Infrastructure.Telegram
{
    [TestClass]
    public class TelegramExchangeRateMessageSenderTests
    {
        private TelegramExchangeRateMessageSender target;
        private Mock<ITelegramMessageSender> telegramMessageSenderMock;

        [TestInitialize]
        public void TestInit()
        {
            this.telegramMessageSenderMock = new Mock<ITelegramMessageSender>();

            this.target = new TelegramExchangeRateMessageSender(this.telegramMessageSenderMock.Object);
        }

        private IEnumerable<ExchangeRate> GetExchangeRates()
        {
            yield return new ExchangeRate()
            {
                ExchangeDateUTC = new DateTime(2021, 11, 14),
                Rate = new ExchangeRateValues(200, 201),
                Provider = ExchangeProvider.DolarSi,
            };

            yield return new ExchangeRate()
            {
                ExchangeDateUTC = new DateTime(2021, 11, 14),
                Rate = new ExchangeRateValues(100, 102),
                Provider = ExchangeProvider.CotizacionCo,
            };
        }

        [TestMethod]
        public async Task It_send_a_message_with_the_latest_exchange_rate_to_the_user()
        {
            // Arrange 
            var exchangeRates = this.GetExchangeRates();
            var queryArgs = new Dictionary<string, string>();
            this.telegramMessageSenderMock
                .Setup(h => h.SendMessage(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Callback<string, Dictionary<string, string>>((interceptedPath, interceptedQueryArgs) => queryArgs = interceptedQueryArgs);

            // Act
            await this.target.SendExchangeRate(exchangeRates, "001");

            // Assert
            var chatIdValue = queryArgs["chat_id"];
            var markdownValue = queryArgs["parse_mode"];
            Assert.AreEqual("001", chatIdValue);
            Assert.AreEqual("MarkdownV2", markdownValue);

            var textValue = queryArgs["text"];

            var expectedDate = "14/11/2021";
            Assert.IsTrue(textValue.Contains(expectedDate), $"Wrong date. Expected: {expectedDate}. Provided message: {textValue}");

            this.VerifyDolarSi(textValue);
            this.VerifyCotizacionCo(textValue);
        }

        private void VerifyDolarSi(string textValue)
        {
            var expectedProvider = "DolarSi";
            var expectedAverage = "Promedio: $200,5";
            var expectedExchangeRate = "$200 / $201";
            Assert.IsTrue(textValue.Contains(expectedProvider), $"Wrong provider. Expected: {expectedProvider}. Provided message: {textValue}");
            Assert.IsTrue(textValue.Contains(expectedAverage), $"Wrong average. Expected: {expectedAverage}. Provided message: {textValue}");
            Assert.IsTrue(textValue.Contains(expectedExchangeRate), $"Wrong exchange rate. Expected: {expectedExchangeRate}. Provided message: {textValue}");
        }

        private void VerifyCotizacionCo(string textValue)
        {
            var expectedProvider = "CotizacionCo";
            var expectedAverage = "Promedio: $101";
            var expectedExchangeRate = "$100 / $102";
            Assert.IsTrue(textValue.Contains(expectedProvider), $"Wrong provider. Expected: {expectedProvider}. Provided message: {textValue}");
            Assert.IsTrue(textValue.Contains(expectedAverage), $"Wrong average. Expected: {expectedAverage}. Provided message: {textValue}");
            Assert.IsTrue(textValue.Contains(expectedExchangeRate), $"Wrong exchange rate. Expected: {expectedExchangeRate}. Provided message: {textValue}");
        }

        [TestMethod]
        public async Task When_there_are_no_exchanges_rates_it_sends_a_message_that_says_so()
        {
            // Arrange
            var queryArgs = new Dictionary<string, string>();
            this.telegramMessageSenderMock
                .Setup(h => h.SendMessage(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Callback<string, Dictionary<string, string>>((interceptedPath, interceptedQueryArgs) => queryArgs = interceptedQueryArgs);

            // Act
            await this.target.SendExchangeRate(new List<ExchangeRate>(), "001");

            // Assert
            var textValue = queryArgs["text"];
            var partialExpectedMessage = "Todavia no hay cotización";

            Assert.IsTrue(textValue.Contains(partialExpectedMessage), $"Wrong message. To contain: {partialExpectedMessage}. Provided message: {textValue}");
        }
    }
}
