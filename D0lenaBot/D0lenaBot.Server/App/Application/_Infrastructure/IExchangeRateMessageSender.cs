using D0lenaBot.Server.App.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IExchangeRateMessageSender
    {
        Task SendExchangeRate(IEnumerable<ExchangeRate> exchangeRates, string chatId);
    }
}
