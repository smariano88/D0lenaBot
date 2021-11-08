using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.NotifyAllExchangeRateCommand
{
    public interface INotifyAllExchangeRateCommand
    {
        Task Send();
    }
}
