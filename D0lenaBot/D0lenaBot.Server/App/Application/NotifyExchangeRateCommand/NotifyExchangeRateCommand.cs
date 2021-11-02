using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.NotifyExchangeRateCommand
{
    // ToDo: 
    // * Notifications should be in a queue, so we have retries for free, among other benefits
    internal class NotifyExchangeRateCommand : INotifyExchangeRateCommand
    {
        private readonly IExchangeRates exchangeRates;
        private readonly INotificationSender notificationSender;
        public NotifyExchangeRateCommand(IExchangeRates exchangeRates, INotificationSender notificationSender)
        {
            this.exchangeRates = exchangeRates;
            this.notificationSender = notificationSender;
        }

        public async Task Send(DateTime date)
        {
            ExchangeRate exchangeRate = await this.exchangeRates.Get(date);
            var chatIds = new List<string>()
            {
                "308011462"
            };

            foreach (var chatId in chatIds)
            {
                await this.notificationSender.Send(exchangeRate, chatId);
            }
        }
    }
}
