using D0lenaBot.Server.App.Domain;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IExchangeRates
    {
        Task Save(ExchangeRate exchangeRate);
        Task<ExchangeRate> GetLatest();
    }
}
