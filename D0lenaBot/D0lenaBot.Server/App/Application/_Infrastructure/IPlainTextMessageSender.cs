using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IPlainTextMessageSender
    {
        Task Send(string message, string chatId);
    }
}
