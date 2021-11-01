using Newtonsoft.Json;
using System;

namespace D0lenaBot.Server.App.Domain
{
    // ToDo: decouple storage properties and decorators from domain model
    public class ExchangeRate
    {
        public ExchangeRate()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = new Guid().ToString();

        public DateTime Date { get; set; }
        public ExchangeRateValues Rate { get; set; }
        public ExchangeProvider Provider { get; set; }
        public string ProviderDescription => this.Provider.ToString();
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
