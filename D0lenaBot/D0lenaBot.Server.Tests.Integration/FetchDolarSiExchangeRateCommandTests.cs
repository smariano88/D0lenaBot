using D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration
{
    [TestClass]
    public class FetchDolarSiExchangeRateCommandTests
    {
        private ExchangeRateRepositoryMock exchangeRateRepositoryMock;

        private IFetchDolarSiExchangeRateCommand target;

        [TestInitialize]
        public void Initialize()
        {
            var testConfigurator = new TestConfigurator();

            this.exchangeRateRepositoryMock = testConfigurator.GetExchangeRatesRepositoryMock();

            this.target = testConfigurator.GetInstance<IFetchDolarSiExchangeRateCommand>();
        }

        [TestMethod]
        public async Task It_saves_the_user_correctly()
        {
            // Act
            await this.target.FetchTodaysExchangeRate();

            // Assert
            var exchangeRates = await this.exchangeRateRepositoryMock.GetAll();
            Assert.AreEqual(1, exchangeRates.Count());

            var exchangeRate = exchangeRates.Single();
            Assert.AreEqual(exchangeRate.Rate.Buy, 194.50m);
            Assert.AreEqual(exchangeRate.Rate.Sell, 197.50m);
            Assert.AreEqual(exchangeRate.Provider, ExchangeProvider.DolarSi);
            Assert.AreEqual(exchangeRate.ExchangeDateUTC, new DateTime(2021, 10, 29, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
