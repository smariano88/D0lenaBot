using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.SendWelcomeMessageCommand
{
    public interface ISendWelcomeMessageCommand
    {
        Task Send(string chatId);
    }
}
