using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace D0lenaBot.Server.App.Infrastructure.CotizacionCo.Services
{
    internal interface ICotizacionCoValuesParser
    {
        decimal ParseRateToDecimal(string content);
        DateTime ParseDate(string content);
    }

    internal class CotizacionCoValuesParser : ICotizacionCoValuesParser
    {
        public DateTime ParseDate(string content)
        {
            var regex = new Regex(@"(\d{1,4}([.\-/])\d{1,2}([.\-/])\d{1,4})");
            var dateMatch = regex.Match(content);
            var rawDate = dateMatch.Groups[0].Value;
            var splitDate = rawDate.Split("/");

            var year = int.Parse(splitDate[2]);
            var month = int.Parse(splitDate[1]);
            var day = int.Parse(splitDate[0]);

            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        public decimal ParseRateToDecimal(string content)
        {
            string normalizedContent = RemoveDollarSign(content);
            normalizedContent = NormalizeDecimalSeparator(normalizedContent);

            return decimal.Parse(normalizedContent, CultureInfo.InvariantCulture);

            string RemoveDollarSign(string val) => val.Replace("$ ", "");
            string NormalizeDecimalSeparator(string val) => val.Replace(",", ".");
        }
    }
}