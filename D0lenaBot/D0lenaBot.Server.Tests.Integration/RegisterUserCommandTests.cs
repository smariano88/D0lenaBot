using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class RegisterUserCommandTests
    {
        private UsersRepositoryMock usersRepositoryMock;
        private TelegramMessageSenderMock telegramSenderMock;

        private IRegisterUserCommand target;

        [TestInitialize]
        public void Initialize()
        {
            var testConfigurator = new TestConfigurator();
            this.usersRepositoryMock = testConfigurator.GetUsersRepositoryMock();
            this.telegramSenderMock = testConfigurator.GetTelegramSenderMock();

            this.target = testConfigurator.GetInstance<IRegisterUserCommand>();
        }

        [TestMethod]
        public async Task It_saves_the_user_correctly()
        {
            // Arrange

            // Act
            await this.target.Register("001", "Mariano", "Soto");

            // Assert
            var user = await this.usersRepositoryMock.GetByChatId("001");
            Assert.AreEqual("Mariano", user.FirstName);
            Assert.AreEqual("Soto", user.LastName);
        }

        [TestMethod]
        public async Task It_sends_a_Welcome_message_to_the_user()
        {
            // Act
            await this.target.Register("001", "Mariano", "Soto");

            // Assert
            var message = this.telegramSenderMock.Messages.Single();

            Assert.AreEqual("/sendMessage", message.Path);

            var textValue = message.QueryArgs["text"];
            Assert.IsTrue(textValue.StartsWith("Te registramos correctamente"));

            var chatIdValue = message.QueryArgs["chat_id"];
            Assert.AreEqual("001", chatIdValue);

            var markdownValue = message.QueryArgs["parse_mode"];
            Assert.AreEqual("MarkdownV2", markdownValue);
        }

        [TestMethod]
        public async Task If_it_send_the_same_user_multiple_times_it_saves_it_only_once()
        {
            // Act
            await this.target.Register("001", "Mariano", "Soto");
            await this.target.Register("001", "Mariano", "");
            await this.target.Register("001", "", "Soto");
            await this.target.Register("001", "", "");

            // Assert
            var users = await this.usersRepositoryMock.GetAll();
            Assert.AreEqual(1, users.Count());
        }

        [TestMethod]
        public async Task If_we_send_the_same_user_multiple_times_it_notifies_only_once()
        {
            // Act
            await this.target.Register("001", "Mariano", "Soto");
            await this.target.Register("001", "Mariano", "");
            await this.target.Register("001", "", "Soto");
            await this.target.Register("001", "", "");

            // Assert
            var message = this.telegramSenderMock.Messages.Single();
            Assert.AreEqual("001", message.QueryArgs["chat_id"]);
        }

        [TestMethod]
        public async Task If_we_send_the_different_users_then_they_are_all_saved_correctly_multiple_times_it_saves_it_only_once()
        {
            // Act
            await this.target.Register("001", "Mariano", "Soto");
            await this.target.Register("002", "Carlos", "Soto");
            await this.target.Register("003", "Tomas", "Soto");
            await this.target.Register("004", "Juan", "Soto");

            // Assert
            var users = await this.usersRepositoryMock.GetAll();

            Assert.AreEqual(4, users.Count());
        }
    }
}
