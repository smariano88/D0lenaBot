using D0lenaBot.Server.App.Application.Infrastructure;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.RemoveUserCommand
{
    // ToDo
    // * Add logs
    internal class RemoveUserCommand : IRemoveUserCommand
    {
        private const string successMessage = "Gracias! Vuelvas prontos!";
        private readonly IUsers users;
        private readonly IPlainTextMessageSender plainTextMessageSender;

        public RemoveUserCommand(IUsers users, IPlainTextMessageSender plainTextMessageSender)
        {
            this.users = users;
            this.plainTextMessageSender = plainTextMessageSender;
        }

        public async Task Remove(string chatId)
        {
            var user = await this.users.GetByChatId(chatId);
            if (user == null)
            {
                return;
            }

            await this.users.Delete(user);

            await this.plainTextMessageSender.Send(successMessage, chatId);
        }
    }
}
