using D0lenaBot.Server.App.Application.NotifyExchangeRateToOnePersonCommand;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class NotifyExchangeRateToOnePersonCommandTests
    {
        private ExchangeRateRepositoryMock exchangeRatesRepositoryMock;
        private TelegramMessageSenderMock telegramSenderMock;

        private INotifyExchangeRateToOnePersonCommand target;

        [TestInitialize]
        public async Task Initialize()
        {
            var testConfigurator = new TestConfigurator();
            this.exchangeRatesRepositoryMock = testConfigurator.GetExchangeRatesRepositoryMock();
            this.telegramSenderMock = testConfigurator.GetTelegramSenderMock();
            await this.SetupExchangeRate();

            this.target = testConfigurator.GetInstance<INotifyExchangeRateToOnePersonCommand>();
        }

        private async Task SetupExchangeRate()
        {
            await this.exchangeRatesRepositoryMock.Save(new ExchangeRate()
            {
                ExchangeDateUTC = DateTime.UtcNow,
                Rate = new ExchangeRateValues(200, 201),
                Provider = ExchangeProvider.DolarSi,
            });
        }

        [TestMethod]
        public async Task It_send_a_message_with_the_latest_exchange_rate_to_the_user()
        {
            // Act
            await this.target.Send("001");

            // Assert
            var message = this.telegramSenderMock.Messages.Single();

            Assert.AreEqual("/sendMessage", message.Path);

            var chatIdValue = message.QueryArgs["chat_id"];
            Assert.AreEqual("001", chatIdValue);

            var markdownValue = message.QueryArgs["parse_mode"];
            Assert.AreEqual("MarkdownV2", markdownValue);

            var textValue = message.QueryArgs["text"];

            Assert.IsTrue(textValue.Contains("14/11/2021"));
            Assert.IsTrue(textValue.Contains("💵 DolarSi"));
            Assert.IsTrue(textValue.Contains("Promedio: $200,5"));
            Assert.IsTrue(textValue.Contains("$200 / $201"));
        }
    }
}
