using D0lenaBot.Server.App.Application.FetchCotizacionCoExchangeRateCommand;
using D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand;
using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Application.NotifyAllExchangeRateCommand;
using D0lenaBot.Server.App.Application.NotifyExchangeRateToOnePersonCommand;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
using D0lenaBot.Server.App.Application.RemoveUserCommand;
using D0lenaBot.Server.App.Application.SendWelcomeMessageCommand;
using D0lenaBot.Server.App.Infrastructure;
using D0lenaBot.Server.App.Infrastructure.CotizacionCo;
using D0lenaBot.Server.App.Infrastructure.CotizacionCo.Services;
using D0lenaBot.Server.App.Infrastructure.DolarSi;
using D0lenaBot.Server.App.Infrastructure.DolarSi.Services;
using D0lenaBot.Server.App.Infrastructure.Telegram;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(D0lenaBot.Server.Startup))]

namespace D0lenaBot.Server
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Commands and queries
            builder.Services.AddScoped<IFetchDolarSiExchangeRateCommand, FetchDolarSiExchangeRateCommand>();
            builder.Services.AddScoped<IFetchCotizacionCoExchangeRateCommand, FetchCotizacionCoExchangeRateCommand>();
            builder.Services.AddScoped<INotifyAllExchangeRateCommand, NotifyAllExchangeRateCommand>();
            builder.Services.AddScoped<INotifyExchangeRateToOnePersonCommand, NotifyExchangeRateToOnePersonCommand>();
            builder.Services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
            builder.Services.AddScoped<ISendWelcomeMessageCommand, SendWelcomeMessageCommand>();
            builder.Services.AddScoped<IRemoveUserCommand, RemoveUserCommand>();
            
            // Repositories
            builder.Services.AddScoped<IExchangeRates, ExchangeRatesRepository>();
            builder.Services.AddScoped<IUsers, UsersRepository>();

            // Exchange Rate providers
            builder.Services.AddScoped<IDolarSiProvider, DolarSiProvider>();
            builder.Services.AddScoped<IDolarSiHtmlLoader, DolarSiHtmlLoader>();
            builder.Services.AddScoped<IDolarSiValuesParser, DolarSiValuesParser>();

            builder.Services.AddScoped<ICotizacionCoProvider, CotizacionCoProvider>();
            builder.Services.AddScoped<ICotizacionCoValuesParser, CotizacionCoValuesParser>();

            // Telegram
            builder.Services.AddScoped<IWelcomeMessageSender, TelegramWelcomeMessageSender>();
            builder.Services.AddScoped<IExchangeRateMessageSender, TelegramExchangeRateMessageSender>();
            builder.Services.AddScoped<IPlainTextMessageSender, TelegramPlainTextMessageSender>();
            builder.Services.AddScoped<ITelegramMessageBuilder, TelegramMessageBuilder>();
            builder.Services.AddScoped<ITelegramMessageSender, TelegramMessageSender>();

            // Misc
            builder.Services.AddSingleton<IEnvironmentVariablesProvider, EnvironmentVariablesProvider>();
        }
    }
}