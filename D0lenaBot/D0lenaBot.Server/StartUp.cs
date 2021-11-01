using D0lenaBot.Server.App.Application.FetchDollarCommand;
using D0lenaBot.Server.App.Application.Infrastructure;
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
            builder.Services.AddScoped<IFetchDollarCommand, FetchDollarCommand>();
            builder.Services.AddScoped<IExchangeRates, ExchangeRatesRepository>();
            builder.Services.AddScoped<IDolarSiProvider, DolarSiProvider>();
            builder.Services.AddScoped<IDolarSiHtmlLoader, DolarSiHtmlLoader>();
        }
    }
}