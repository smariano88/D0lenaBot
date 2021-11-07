using System;
using System.Globalization;

namespace D0lenaBot.Server.App.Infrastructure
{
    internal interface IDolarSiValuesParser
    {
        decimal ParseRateToDecimal(string content);
        DateTime ParseDate(string content);
    }

    internal class DolarSiValuesParser : IDolarSiValuesParser
    {
        public DateTime ParseDate(string content)
        {
            var rawDate = content.Split(" ")[1];
            var splitDate = rawDate.Split("/");

            var year = int.Parse(splitDate[2]);
            var month = int.Parse(splitDate[1]);
            var day = int.Parse(splitDate[0]);

            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        string RemoveDollarSign(string val) => val.Replace("$ ", "");
        string NormalizeDecimalSeparator(string val) => val.Replace(",", ".");

        public decimal ParseRateToDecimal(string content)
        {
            string normalizedContent = this.RemoveDollarSign(content);
            normalizedContent = this.NormalizeDecimalSeparator(normalizedContent);

            return decimal.Parse(normalizedContent, CultureInfo.InvariantCulture);
        }
    }
}