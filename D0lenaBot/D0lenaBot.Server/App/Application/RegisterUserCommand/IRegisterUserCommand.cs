using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.RegisterUserCommand
{
    public interface IRegisterUserCommand
    {
        Task Register(string chatId, string firstName, string lastName);
    }
}
