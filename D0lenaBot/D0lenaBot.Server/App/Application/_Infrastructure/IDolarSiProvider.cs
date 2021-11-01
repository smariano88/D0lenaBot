using D0lenaBot.Server.App.Domain;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IDolarSiProvider
    {
        ExchangeRate GetCurrentExchangeRate();
    }
}
