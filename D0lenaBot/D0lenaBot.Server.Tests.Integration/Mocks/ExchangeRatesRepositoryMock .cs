using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration.Mocks
{
    public class ExchangeRateRepositoryMock : IExchangeRates
    {
        private List<ExchangeRate> ExchangeRates = new List<ExchangeRate>();
        public async Task<ExchangeRate> GetLatest()
        {
            return this.ExchangeRates.OrderByDescending(e => e.CreatedDateUTC).FirstOrDefault();
        }

        public async Task Save(ExchangeRate exchangeRate)
        {
            this.ExchangeRates.Add(exchangeRate);
        }
    }
}
