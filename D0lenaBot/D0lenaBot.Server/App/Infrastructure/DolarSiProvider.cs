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
        private readonly IDolarSiValuesParser dolarSiValuesParser;

        public DolarSiProvider(IDolarSiHtmlLoader dolarSiHtmlLoader, IDolarSiValuesParser dolarSiValuesParser)
        {
            this.dolarSiHtmlLoader = dolarSiHtmlLoader;
            this.dolarSiValuesParser = dolarSiValuesParser;
        }

        public async Task<ExchangeRate> GetCurrentExchangeRate()
        {
            HtmlDocument document = await this.dolarSiHtmlLoader.Load(URL);

            var rate = this.GetExchangeRate(document);
            var date = this.GetDate(document);

            return new ExchangeRate()
            {
                ExchangeDateUTC = date,
                Rate = rate,
                Provider = ExchangeProvider.DolarSi
            };
        }

        private ExchangeRateValues GetExchangeRate(HtmlDocument document)
        {
            var container = document.DocumentNode
                                    .Descendants("div").Where(x => x.Attributes["class"].Value == "cont-cv")
                                    .First();

            var buy = GetDecimalRate(container, "comp");
            var sell = GetDecimalRate(container, "vent");

            return new ExchangeRateValues(buy, sell);

            decimal GetDecimalRate(HtmlNode container, string selector)
            {
                var valueContainer = container.Descendants("div").Where(x => x.Attributes["class"].Value == selector).First();

                var content = valueContainer.Descendants("div").Where(x => x.Attributes["class"].Value == "val-cv").First().InnerHtml;

                return this.dolarSiValuesParser.ParseRateToDecimal(content);
            }
        }

        private DateTime GetDate(HtmlDocument document)
        {
            var container = document.DocumentNode
                                   .Descendants("div").Where(x => x.Attributes["class"].Value == "fecha")
                                   .First();
            var content = container.InnerText;

            return this.dolarSiValuesParser.ParseDate(content);
        }
    }
}
