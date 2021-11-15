using D0lenaBot.Server.App.Application.SendWelcomeMessageCommand;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class SendWelcomeMessageCommandTests
    {
        private TelegramMessageSenderMock telegramSenderMock;

        private ISendWelcomeMessageCommand target;

        [TestInitialize]
        public void Initialize()
        {
            var testConfigurator = new TestConfigurator();
            this.telegramSenderMock = testConfigurator.GetTelegramSenderMock();

            this.target = testConfigurator.GetInstance<ISendWelcomeMessageCommand>();
        }

        [TestMethod]
        public async Task It_saves_the_user_correctly()
        {
            // Act
            await this.target.Send("001");

            // Assert
            var user = this.telegramSenderMock.Messages.Single();
            Assert.IsTrue(user.QueryArgs["text"].Contains("Bienvenido"));
            Assert.IsTrue(user.QueryArgs["text"].Contains("/subscribe"));
            Assert.IsTrue(user.QueryArgs["text"].Contains("/stop"));
            Assert.IsTrue(user.QueryArgs["text"].Contains("/coti"));
        }
    }
}
