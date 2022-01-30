using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Infrastructure.DolarSi.Services;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using D0lenaBot.Server.Tests.Integration.Mocks;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System.IO;

namespace D0lenaBot.Server.Tests.Integration
{
    // ToDo: Move one folder up
    public class TestConfigurator
    {
        private IHost host;
        private IWebJobsBuilder builder;

        public TestConfigurator()
        {
            this.host = new HostBuilder()
                .ConfigureWebJobs((x) => this.ConfigureDependencies(x))
                .Build();
        }

        public T GetInstance<T>() where T : class
        {
            return this.host.Services.GetRequiredService<T>();
        }

        private void ConfigureDependencies(IWebJobsBuilder builder)
        {
            this.builder = builder;
            var startup = new Startup();
            startup.Configure(builder);
            builder.Services.AddScoped<IUsers, UsersRepositoryMock>();
            builder.Services.AddScoped<IExchangeRates, ExchangeRateRepositoryMock>();
            builder.Services.AddScoped<ITelegramMessageSender, TelegramMessageSenderMock>();

            this.ConfigureHtmlLoader();
        }

        private void ConfigureHtmlLoader()
        {
            var dolarSiHtmlLoaderMock = new Mock<IDolarSiHtmlLoader>();
            dolarSiHtmlLoaderMock
                .Setup(m => m.Load("https://www.dolarsi.com/func/cotizacion_dolar_blue.php"))
                .ReturnsAsync(this.CreateMockHtmlDocument("DolarSi.html"));

            dolarSiHtmlLoaderMock
                .Setup(m => m.Load("https://www.cotizacion.co/rosario/"))
                .ReturnsAsync(this.CreateMockHtmlDocument("CotizacionCo.html"));

            this.builder.Services.AddScoped<IDolarSiHtmlLoader>((something) => dolarSiHtmlLoaderMock.Object);
        }


        private HtmlDocument CreateMockHtmlDocument(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "Static", fileName);

            var doc = new HtmlDocument();
            doc.Load(path);
            return doc;
        }
    }

    public static class TestConfiguratorExtensions
    {
        public static UsersRepositoryMock GetUsersRepositoryMock(this TestConfigurator testConfigurator)
        {
            return testConfigurator.GetInstance<IUsers>() as UsersRepositoryMock;
        }
        public static ExchangeRateRepositoryMock GetExchangeRatesRepositoryMock(this TestConfigurator testConfigurator)
        {
            return testConfigurator.GetInstance<IExchangeRates>() as ExchangeRateRepositoryMock;
        }
        internal static TelegramMessageSenderMock GetTelegramSenderMock(this TestConfigurator testConfigurator)
        {
            return testConfigurator.GetInstance<ITelegramMessageSender>() as TelegramMessageSenderMock;
        }
    }
}
