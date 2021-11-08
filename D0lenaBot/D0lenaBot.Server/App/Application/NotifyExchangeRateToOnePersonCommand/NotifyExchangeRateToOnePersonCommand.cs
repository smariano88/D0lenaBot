using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.NotifyExchangeRateToOnePersonCommand
{
    // ToDo: 
    // * Notifications should be in a queue, so we have retries for free, among other benefits
    internal class NotifyExchangeRateToOnePersonCommand : INotifyExchangeRateToOnePersonCommand
    {
        private readonly IExchangeRates exchangeRates;
        private readonly IExchangeRateMessageSender notificationSender;

        public NotifyExchangeRateToOnePersonCommand(IExchangeRates exchangeRates, IExchangeRateMessageSender notificationSender)
        {
            this.exchangeRates = exchangeRates;
            this.notificationSender = notificationSender;
        }

        public async Task Send(string chatId)
        {
            ExchangeRate exchangeRate = await this.exchangeRates.GetLatest();
            await this.notificationSender.SendExchangeRate(exchangeRate, chatId);
        }
    }
}
