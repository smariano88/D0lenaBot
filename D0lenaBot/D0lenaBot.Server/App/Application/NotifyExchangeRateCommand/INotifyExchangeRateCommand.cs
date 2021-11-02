using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.NotifyExchangeRateCommand
{
    public interface INotifyExchangeRateCommand
    {
        Task Send(DateTime date);
    }
}
