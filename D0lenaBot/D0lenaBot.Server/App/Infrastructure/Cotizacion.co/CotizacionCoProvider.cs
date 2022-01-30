using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.App.Infrastructure.CotizacionCo.Services;
using D0lenaBot.Server.App.Infrastructure.DolarSi.Services;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure.CotizacionCo
{
    internal class CotizacionCoProvider : ICotizacionCoProvider
    {
        private const string URL = "https://www.cotizacion.co/rosario/";
        private readonly IDolarSiHtmlLoader htmlLoader;
        private readonly ICotizacionCoValuesParser cotizacionCoValuesParser;

        public CotizacionCoProvider(IDolarSiHtmlLoader dolarSiHtmlLoader, ICotizacionCoValuesParser cotizacionCoValuesParser)
        {
            this.htmlLoader = dolarSiHtmlLoader;
            this.cotizacionCoValuesParser = cotizacionCoValuesParser;
        }

        public async Task<ExchangeRate> GetCurrentExchangeRate()
        {
            HtmlDocument document = await this.htmlLoader.Load(URL);

            var rate = this.GetExchangeRate(document);
            var date = this.GetDate(document);

            return new ExchangeRate()
            {
                ExchangeDateUTC = date,
                Rate = rate,
                Provider = ExchangeProvider.CotizacionCo
            };
        }

        private ExchangeRateValues GetExchangeRate(HtmlDocument document)
        {
            var container = document.DocumentNode
                                    .Descendants("h2")
                                    .Where(x => x.Attributes["class"]?.Value == "pricetxt");

            var buy = GetDecimalRate(container, 0);
            var sell = GetDecimalRate(container, 1);

            return new ExchangeRateValues(buy, sell);

            decimal GetDecimalRate(IEnumerable<HtmlNode> container, int position)
            {
                var valueContainer = container.ElementAt(position).InnerHtml;

                return this.cotizacionCoValuesParser.ParseRateToDecimal(valueContainer);
            }
        }

        private DateTime GetDate(HtmlDocument document)
        {
            var container = document.DocumentNode
                                   .Descendants("h2").First();
                                               
            var content = container.InnerText;

            return this.cotizacionCoValuesParser.ParseDate(content);
        }
    }
}
