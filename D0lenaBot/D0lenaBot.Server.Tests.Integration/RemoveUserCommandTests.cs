using D0lenaBot.Server.App.Application.RemoveUserCommand;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class RemoveUserCommandTests
    {
        private UsersRepositoryMock usersRepositoryMock;
        private TelegramMessageSenderMock telegramSenderMock;

        private IRemoveUserCommand target;

        [TestInitialize]
        public async Task Initialize()
        {
            var testConfigurator = new TestConfigurator();
            this.usersRepositoryMock = testConfigurator.GetUsersRepositoryMock();
            this.telegramSenderMock = testConfigurator.GetTelegramSenderMock();
            await this.SetupUser();
            this.target = testConfigurator.GetInstance<IRemoveUserCommand>();

        }

        private async Task SetupUser()
        {
            await this.usersRepositoryMock.Save(new App.Domain.User()
            {
                Id = "001",
                FirstName = "Mariano",
                LastName = "Soto",
            });
        }

        [TestMethod]
        public async Task It_removes_the_user_correctly()
        {
            // Act
            await this.target.Remove("001");

            // Assert
            var user = await this.usersRepositoryMock.GetByChatId("001");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task It_sends_a_Goodbye_message_to_the_user()
        {
            // Act
            await this.target.Remove("001");

            // Assert
            var message = this.telegramSenderMock.Messages.Single();

            Assert.AreEqual("/sendMessage", message.Path);

            var textValue = message.QueryArgs["text"];
            Assert.IsTrue(textValue.StartsWith("Gracias"));

            var chatIdValue = message.QueryArgs["chat_id"];
            Assert.AreEqual("001", chatIdValue);

            var markdownValue = message.QueryArgs["parse_mode"];
            Assert.AreEqual("MarkdownV2", markdownValue);
        }

        [TestMethod]
        public async Task If_we_remove_the_user_multiple_times_it_removes_it_correctly_and_it_does_not_throw_an_exception()
        {
            // Act
            await this.target.Remove("001");
            await this.target.Remove("001");
            await this.target.Remove("001");
            await this.target.Remove("001");

            // Assert
            var users = await this.usersRepositoryMock.GetAll();
            Assert.AreEqual(0, users.Count());
        }

        [TestMethod]
        public async Task If_we_remove_the_user_user_multiple_times_it_notifies_only_once()
        {
            // Act
            await this.target.Remove("001");
            await this.target.Remove("001");
            await this.target.Remove("001");
            await this.target.Remove("001");

            // Assert
            var message = this.telegramSenderMock.Messages.Single();
            Assert.AreEqual("001", message.QueryArgs["chat_id"]);
        }
    }
}
