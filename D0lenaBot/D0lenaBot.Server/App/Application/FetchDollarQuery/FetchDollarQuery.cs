using D0lenaBot.Server.App.Application.Infrastructure;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchDollarQuery
{
    internal class FetchDollarQuery : IFetchDollarQuery
    {
        private readonly IDolarSiProvider dollarSiProvider;
        private readonly IExchangeRates exchangeRates;
        public FetchDollarQuery(IDolarSiProvider dollarSiProvider, IExchangeRates exchangeRates)
        {
            this.dollarSiProvider = dollarSiProvider;
            this.exchangeRates = exchangeRates;
        }

        public async Task Fetch(DateTime date)
        {
            var currentExchangeRate = await this.dollarSiProvider.GetCurrentExchangeRate();

            await this.exchangeRates.Save(currentExchangeRate);
        }
    }
}
