using D0lenaBot.Server.App.Application.Infrastructure;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchDollarCommand
{
    internal class FetchDollarCommand : IFetchDollarCommand
    {
        private readonly IDolarSiProvider dollarSiProvider;
        private readonly IExchangeRates exchangeRates;
        public FetchDollarCommand(IDolarSiProvider dollarSiProvider, IExchangeRates exchangeRates)
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
