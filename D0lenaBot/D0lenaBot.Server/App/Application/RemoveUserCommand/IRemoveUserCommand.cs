using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.RemoveUserCommand
{
    public interface IRemoveUserCommand
    {
        Task Remove(string chatId);
    }
}
