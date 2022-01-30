using D0lenaBot.Server.App.Application.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.NotifyAllExchangeRateCommand
{
    // ToDo: 
    // * Notifications should be in a queue, so we have retries for free, among other benefits
    internal class NotifyAllExchangeRateCommand : INotifyAllExchangeRateCommand
    {
        private readonly IExchangeRates exchangeRates;
        private readonly IUsers users;
        private readonly IExchangeRateMessageSender notificationSender;
        public NotifyAllExchangeRateCommand(IExchangeRates exchangeRates, IUsers users, IExchangeRateMessageSender notificationSender)
        {
            this.exchangeRates = exchangeRates;
            this.users = users;
            this.notificationSender = notificationSender;
        }

        public async Task Send()   
        {
            var latestExchangeRate = await this.exchangeRates.GetLatest();
            if (latestExchangeRate == null)
            {
                // ToDo: send "No hay cotizacion" message;
                return;
            }

            var exchangesRates = await this.exchangeRates.GetExchangeRateFor(latestExchangeRate.ExchangeDateUTC);
            var chatIds = (await this.users.GetAll()).Select(u => u.Id);

            foreach (var chatId in chatIds)
            {
                await this.notificationSender.SendExchangeRate(exchangesRates, chatId);
            }
        }
    }
}
