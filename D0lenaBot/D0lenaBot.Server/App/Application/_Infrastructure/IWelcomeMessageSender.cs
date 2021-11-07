using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IWelcomeMessageSender
    {
        Task Send(string chatId);
    }
}
