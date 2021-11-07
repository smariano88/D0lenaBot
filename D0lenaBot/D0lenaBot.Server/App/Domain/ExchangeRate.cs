using Newtonsoft.Json;
using System;

namespace D0lenaBot.Server.App.Domain
{
    // ToDo: 
    // * Decouple storage properties and decorators from domain model.
    // * Take a look at timezones for props: DateUTC and CreatedDateUTC
    public class ExchangeRate
    {
        public ExchangeRate()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime ExchangeDateUTC { get; set; }
        public DateTime CreatedDateUTC { get; set; } = DateTime.UtcNow;
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
        public decimal Average => Math.Round((this.Buy + this.Sell) / 2, 2, MidpointRounding.AwayFromZero);
    }

    public enum ExchangeProvider
    {
        DolarSi = 1,
    }
}
