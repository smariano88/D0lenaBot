using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure
{
    internal class DolarSiProvider : IDolarSiProvider
    {
        public async Task<ExchangeRate> GetCurrentExchangeRate()
        {
            return new ExchangeRate()
            {
                Date = DateTime.Now,
                Rate = new ExchangeRateValues(195, 200),
                Provider = ExchangeProvider.DolarSi
            };
        }
    }
}
