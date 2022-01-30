using D0lenaBot.Server.App.Application.FetchCotizacionCoExchangeRateCommand;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class FetchCotizacionCoExchangeRateCommandTests
    {
        private ExchangeRateRepositoryMock exchangeRateRepositoryMock;

        private IFetchCotizacionCoExchangeRateCommand target;

        [TestInitialize]
        public void Initialize()
        {
            var testConfigurator = new TestConfigurator();

            this.exchangeRateRepositoryMock = testConfigurator.GetExchangeRatesRepositoryMock();

            this.target = testConfigurator.GetInstance<IFetchCotizacionCoExchangeRateCommand>();
        }

        [TestMethod]
        public async Task It_saves_the_exchange_rate_correctly()
        {
            // Act
            await this.target.FetchTodaysExchangeRate();

            // Assert
            var exchangeRates = await this.exchangeRateRepositoryMock.GetAll();
            Assert.AreEqual(1, exchangeRates.Count());

            var exchangeRate = exchangeRates.Single();
            Assert.AreEqual(exchangeRate.Rate.Buy, 206.50m);
            Assert.AreEqual(exchangeRate.Rate.Sell, 213);
            Assert.AreEqual(exchangeRate.Provider, ExchangeProvider.CotizacionCo);
            Assert.AreEqual(exchangeRate.ExchangeDateUTC, new DateTime(2022, 1, 30, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
