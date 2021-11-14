using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration.Mocks
{
    public class ExchangeRateRepositoryMock : IExchangeRates
    {
        public Task<ExchangeRate> GetLatest()
        {
            throw new NotImplementedException();
        }

        public Task Save(ExchangeRate exchangeRate)
        {
            throw new NotImplementedException();
        }
    }
}
