using D0lenaBot.Server.App.Application.Infrastructure;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchCotizacionCoExchangeRateCommand
{
    internal class FetchCotizacionCoExchangeRateCommand : IFetchCotizacionCoExchangeRateCommand
    {
        private readonly ICotizacionCoProvider cotizacionCoProvider;
        private readonly IExchangeRates exchangeRates;

        public FetchCotizacionCoExchangeRateCommand(ICotizacionCoProvider cotizacionCoProvider, IExchangeRates exchangeRates)
        {
            this.cotizacionCoProvider = cotizacionCoProvider;
            this.exchangeRates = exchangeRates;
        }

        public async Task FetchTodaysExchangeRate()
        {
            var cotizacionCo = await this.cotizacionCoProvider.GetCurrentExchangeRate();
            await this.exchangeRates.Save(cotizacionCo);
        }
    }
}
