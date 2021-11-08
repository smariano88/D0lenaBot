using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.RegisterUserCommand
{
    // ToDo
    // * Add logs
    internal class RegisterUserCommand : IRegisterUserCommand
    {
        private readonly IUsers users;
        private readonly IUserRegisteredMessageSender userRegisteredMessageSender;

        public RegisterUserCommand(IUsers users, IUserRegisteredMessageSender userRegisteredMessageSender)
        {
            this.users = users;
            this.userRegisteredMessageSender = userRegisteredMessageSender;
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
                ChatId = chatId,
                FirstName = firstName, 
                LastName = lastName
            });

            await this.userRegisteredMessageSender.Send(chatId);
        }
    }
}
