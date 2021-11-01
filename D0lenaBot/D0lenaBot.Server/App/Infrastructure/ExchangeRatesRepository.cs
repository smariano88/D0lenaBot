using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;

namespace D0lenaBot.Server.App.Infrastructure
{
    internal class ExchangeRatesRepository : IExchangeRates
    {
        public void Save(ExchangeRate exchangeRate)
        {
            Console.WriteLine("saved");
            Console.WriteLine(exchangeRate);
        }
    }
}
