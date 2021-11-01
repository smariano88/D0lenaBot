using D0lenaBot.Server.App.Application.Infrastructure;
using System;

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

        public void Fetch(DateTime date)
        {
            var currentExchangeRate = this.dollarSiProvider.GetCurrentExchangeRate();

            this.exchangeRates.Save(currentExchangeRate);
        }
    }
}
