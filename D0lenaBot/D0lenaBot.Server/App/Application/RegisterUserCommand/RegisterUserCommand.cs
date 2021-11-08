using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.RegisterUserCommand
{
    // ToDo
    // * Add logs
    internal class RegisterUserCommand : IRegisterUserCommand
    {
        private const string successMessage = "Te registramos correctamente!";
        private readonly IUsers users;
        private readonly IPlainTextMessageSender plainTextMessageSender;

        public RegisterUserCommand(IUsers users, IPlainTextMessageSender plainTextMessageSender)
        {
            this.users = users;
            this.plainTextMessageSender = plainTextMessageSender;
        }

        public async Task Register(string chatId, string firstName, string lastName)

        {
            var user = await this.users.GetByChatId(chatId);
            if (user != null)
            {
                return;
            }

            await this.users.Save(new User()
            {
                Id = chatId,
                FirstName = firstName, 
                LastName = lastName
            });

            await this.plainTextMessageSender.Send(successMessage, chatId);
        }
    }
}
