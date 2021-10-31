using D0lenaBot.Server.App.Application.FetchDollarQuery;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(D0lenaBot.Server.Startup))]

namespace D0lenaBot.Server
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IFetchDollarQuery, FetchDollarQuery>();
        }
    }
}