using D0lenaBot.Server.App.Application.Infrastructure;
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
            var latestExchangeRate = await this.exchangeRates.GetLatest();
            if (latestExchangeRate == null)
            {
                // ToDo: send "No hay cotizacion" message;
                return;
            }

            var exchangesRates = await this.exchangeRates.GetExchangeRateFor(latestExchangeRate.ExchangeDateUTC);
        
            await this.notificationSender.SendExchangeRate(exchangesRates, chatId);
        }
    }
}
