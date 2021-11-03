using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.RegisterUserCommand
{
    // ToDo
    // * Add logs
    internal class RegisterUserCommand : IRegisterUserCommand
    {
        private readonly IUsers users;
        public RegisterUserCommand(IUsers users)
        {
            this.users = users;
        }

        public async Task Register(string chatId)
        {
            var user = await this.users.GetByChatId(chatId);
            if (user != null)
            {
                return;
            }

            await this.users.Save(new User()
            {
                ChatId = chatId
            });
        }
    }
}
