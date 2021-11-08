using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IUserRegisteredMessageSender
    {
        Task Send(string chatId);
    }
}
