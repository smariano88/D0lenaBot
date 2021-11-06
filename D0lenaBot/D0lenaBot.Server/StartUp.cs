using D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand;
using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Application.NotifyExchangeRateCommand;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
using D0lenaBot.Server.App.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(D0lenaBot.Server.Startup))]

namespace D0lenaBot.Server
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IFetchDolarSiExchangeRateCommand, FetchDolarSiExchangeRateCommand>();
            builder.Services.AddScoped<INotifyExchangeRateCommand, NotifyExchangeRateCommand>();
            builder.Services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();

            builder.Services.AddScoped<IExchangeRates, ExchangeRatesRepository>();
            builder.Services.AddScoped<IUsers, UsersRepository>();
            builder.Services.AddScoped<IDolarSiProvider, DolarSiProvider>();
            builder.Services.AddScoped<IDolarSiHtmlLoader, DolarSiHtmlLoader>();
            builder.Services.AddScoped<IDolarSiValuesParser, DolarSiValuesParser>();
            builder.Services.AddScoped<INotificationSender, TelegramNotificationSender>();
            builder.Services.AddSingleton<IEnvironmentVariablesProvider, EnvironmentVariablesProvider>();
        }
    }
}