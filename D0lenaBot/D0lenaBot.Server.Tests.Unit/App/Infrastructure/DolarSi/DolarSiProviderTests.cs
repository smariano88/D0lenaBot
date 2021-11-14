using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.App.Infrastructure.DolarSi;
using D0lenaBot.Server.App.Infrastructure.DolarSi.Services;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace D0lenaBot.Server.UnitTest.App.Infrastructure.DolarSi
{
    // ToDo: more like an integration test, move to a different project
    [TestClass]
    public class DolarSiProviderTests
    {
        [TestMethod]
        public async Task It_returns_the_correct_exchange_rate()
        {
            // Arrange
            var mockLoader = new Mock<IDolarSiHtmlLoader>();
            HtmlDocument doc = CreateMockHtmlDocument();

            mockLoader.Setup(m => m.Load(It.IsAny<string>()))
                      .ReturnsAsync(doc);
            var target = new DolarSiProvider(mockLoader.Object, new DolarSiValuesParser());

            // Act
            var result = await target.GetCurrentExchangeRate();

            //Assert 
            Assert.AreEqual(result.Rate.Buy, 194.50m);
            Assert.AreEqual(result.Rate.Sell, 197.50m);
            Assert.AreEqual(result.Provider, ExchangeProvider.DolarSi);
            Assert.AreEqual(result.ExchangeDateUTC, new DateTime(2021, 10, 29, 0, 0, 0, DateTimeKind.Utc));
        }

        private static HtmlDocument CreateMockHtmlDocument()
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "App", "Infrastructure", "DolarSi", "MockHtml.html");

            var doc = new HtmlDocument();
            doc.Load(path);
            return doc;
        }
    }
}
