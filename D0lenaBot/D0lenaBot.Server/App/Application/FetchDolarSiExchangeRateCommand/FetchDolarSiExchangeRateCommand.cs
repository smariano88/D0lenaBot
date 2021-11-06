using D0lenaBot.Server.App.Application.Infrastructure;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand
{
    internal class FetchDolarSiExchangeRateCommand : IFetchDolarSiExchangeRateCommand

    {
        private readonly IDolarSiProvider dollarSiProvider;
        private readonly IExchangeRates exchangeRates;
        public FetchDolarSiExchangeRateCommand(IDolarSiProvider dollarSiProvider, IExchangeRates exchangeRates)
        {
            this.dollarSiProvider = dollarSiProvider;
            this.exchangeRates = exchangeRates;
        }

        public async Task FetchTodaysExchangeRate()
        {
            var currentExchangeRate = await this.dollarSiProvider.GetCurrentExchangeRate();

            await this.exchangeRates.Save(currentExchangeRate);
        }
    }
}
