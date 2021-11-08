using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.NotifyExchangeRateToOnePersonCommand
{
    public interface INotifyExchangeRateToOnePersonCommand
    {
        Task Send(string chatId);
    }
}
