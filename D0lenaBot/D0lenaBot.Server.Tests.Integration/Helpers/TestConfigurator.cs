using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using D0lenaBot.Server.Tests.Integration.Mocks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D0lenaBot.Server.Tests.Integration
{
    public class TestConfigurator
    {
        private IHost host;

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
            var startup = new Startup();
            startup.Configure(builder);
            builder.Services.AddScoped<IUsers, UsersRepositoryMock>();
            builder.Services.AddScoped<IExchangeRates, ExchangeRateRepositoryMock>();
            builder.Services.AddScoped<ITelegramMessageSender, TelegramMessageSenderMock>();
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
