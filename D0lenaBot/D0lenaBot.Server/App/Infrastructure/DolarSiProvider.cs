using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using HtmlAgilityPack;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure
{
    internal class DolarSiProvider : IDolarSiProvider
    {
        private const string URL = "https://www.dolarsi.com/func/cotizacion_dolar_blue.php";
        private readonly IDolarSiHtmlLoader dolarSiHtmlLoader;
        public DolarSiProvider(IDolarSiHtmlLoader dolarSiHtmlLoader)
        {
            this.dolarSiHtmlLoader = dolarSiHtmlLoader;
        }

        public async Task<ExchangeRate> GetCurrentExchangeRate()
        {
            HtmlDocument document = await this.dolarSiHtmlLoader.Load(URL);

            var rate = this.GetExchangeRate(document);
            var date = this.GetDate(document);

            return new ExchangeRate()
            {
                Date = date,
                Rate = rate,
                Provider = ExchangeProvider.DolarSi
            };
        }

        private DateTime GetDate(HtmlDocument document)
        {
            var container = document.DocumentNode
                                   .Descendants("div").Where(x => x.Attributes["class"].Value == "fecha")
                                   .First();

            var rawDate = container.InnerText.Split(" ")[1];
            var splitDate = rawDate.Split("/");
            var year = int.Parse(splitDate[2]);
            var month = int.Parse(splitDate[1]);
            var day = int.Parse(splitDate[0]);

            var date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);

            return date;
        }

        private ExchangeRateValues GetExchangeRate(HtmlDocument document)
        {
            var container = document.DocumentNode
                                    .Descendants("div").Where(x => x.Attributes["class"].Value == "cont-cv")
                                    .First();

            var buy = this.GetValue(container, "comp");
            var sell = this.GetValue(container, "vent");

            return new ExchangeRateValues(buy, sell);
        }

        private decimal GetValue(HtmlNode container, string selector)
        {
            var valueContainer = container.Descendants("div").Where(x => x.Attributes["class"].Value == selector).First();

            var content = valueContainer.Descendants("div").Where(x => x.Attributes["class"].Value == "val-cv").First().InnerHtml;

            var contentWithoutSign = content.Replace("$ ", "");
            return decimal.Parse(contentWithoutSign);
        }
    }
}
