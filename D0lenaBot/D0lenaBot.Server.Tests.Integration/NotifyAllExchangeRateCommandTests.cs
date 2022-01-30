using D0lenaBot.Server.App.Application.NotifyAllExchangeRateCommand;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.Tests.Integration.Helpers;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class NotifyAllExchangeRateCommandTests
    {
        private ExchangeRateRepositoryMock exchangeRatesRepositoryMock;
        private UsersRepositoryMock usersRepositoryMock;
        private TelegramMessageSenderMock telegramSenderMock;

        private INotifyAllExchangeRateCommand target;

        [TestInitialize]
        public async Task Initialize()
        {
            var testConfigurator = new TestConfigurator();
            this.exchangeRatesRepositoryMock = testConfigurator.GetExchangeRatesRepositoryMock();
            this.usersRepositoryMock = testConfigurator.GetUsersRepositoryMock();
            this.telegramSenderMock = testConfigurator.GetTelegramSenderMock();
            await this.SetupExchangeRate();
            await this.SetupUsers();

            this.target = testConfigurator.GetInstance<INotifyAllExchangeRateCommand>();
        }

        private async Task SetupExchangeRate()
        {
            await this.exchangeRatesRepositoryMock.Save(new ExchangeRate()
            {
                ExchangeDateUTC = new DateTime(2021, 11, 14),
                Rate = new ExchangeRateValues(200, 201),
                Provider = ExchangeProvider.DolarSi,
            });
        }

        private async Task SetupUsers()
        {
            await this.usersRepositoryMock.Save(new User()
            {
                Id = "001",
                FirstName = "Mariano",
                LastName = "Soto",
            });

            await this.usersRepositoryMock.Save(new User()
            {
                Id = "002",
                FirstName = "Pedro",
                LastName = "Soto",
            });
        }

        // ToDo: think about this test. Should we assert on the message?
        [TestMethod]
        public async Task It_send_a_message_with_the_latest_exchange_rate_to_all_the_users()
        {
            // Act
            await this.target.Send();

            // Assert
            var messages = this.telegramSenderMock.Messages;

            Assert.AreEqual(2, messages.Count);

            var chatId1Value = messages[0].QueryArgs["chat_id"];
            Assert.AreEqual("001", chatId1Value);

            var chatId2Value = messages[1].QueryArgs["chat_id"];
            Assert.AreEqual("002", chatId2Value);
            foreach (var message in messages)
            {
                var textValue = message.QueryArgs["text"];

                ExtendedAssert.ContainsTextOnlyOnce(textValue, "14/11/2021");
                ExtendedAssert.ContainsTextOnlyOnce(textValue, "DolarSi");
                ExtendedAssert.ContainsTextOnlyOnce(textValue, "Promedio: $200,5");
                ExtendedAssert.ContainsTextOnlyOnce(textValue, "$200 / $201");
            }
        }
    }
}
