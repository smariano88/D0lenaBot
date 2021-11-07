using D0lenaBot.Server.App.Application.Infrastructure;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.SendWelcomeMessageCommand
{
    internal class SendWelcomeMessageCommand : ISendWelcomeMessageCommand
    {
        private readonly IWelcomeMessageSender welcomeMessageSender;

        public SendWelcomeMessageCommand(IWelcomeMessageSender welcomeMessageSender)
        {
            this.welcomeMessageSender = welcomeMessageSender;
        }

        public async Task Send(string chatId)
        {
            await this.welcomeMessageSender.Send(chatId);
        }
    }
}
