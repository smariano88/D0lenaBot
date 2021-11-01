using System;

namespace D0lenaBot.Server.App.Domain
{
    public class ExchangeRate
    {
        public ExchangeRate()
        {
        }

        public ExchangeRateValues Rate { get; set; }
        public DateTime Date { get; set; }
        public ExchangeProvider Provider { get; set; }
    }

    public struct ExchangeRateValues
    {
        public ExchangeRateValues(decimal buy, decimal sell)
        {
            this.Buy = buy;
            this.Sell = sell;
        }

        public decimal Buy { get; }
        public decimal Sell { get; }
    }

    public enum ExchangeProvider
    {
        DolarSi = 1,
    }
}
