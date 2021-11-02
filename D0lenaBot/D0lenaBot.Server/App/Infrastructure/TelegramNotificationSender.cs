using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System.Net.Http;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure
{
    // ToDo: 
    // Apply SRP
    // Create unit test
    // move some of the const to the env file
    internal class TelegramNotificationSender : INotificationSender
    {
        public const string baseUrl = "https://api.telegram.org/bot";
        public const string token = "";
        public const string path = "/sendMessage?";
        private const string textParamName = "text=";
        private const string chatIdParamName = "chat_id=";
        private const string parse_mode = "parse_mode=MarkdownV2";

        public async Task Send(ExchangeRate exchangeRate, string chatId)
        {
            string chatIdParameter = chatIdParamName + chatId;
            string textParameter = textParamName + this.GetText(exchangeRate);

            string finalUrl = baseUrl + token + path + $"{parse_mode}&{chatIdParameter}&{textParameter}";

            HttpClient req = new HttpClient();
            await req.GetAsync(finalUrl);
        }

        private string GetText(ExchangeRate exchangeRate)
        {
            string newLine = "%0A";
            string header = "";// "*Cotizacion para Rosario*" + newLine;
            string fecha = $"_Fecha coti_: {exchangeRate.DateUTC.ToString("dd/MM/yyyy")}";
            string tab = "      ";
            string exchangeTemplate = "💵 *{3}* " + newLine + tab + "${0} / ${1}" + newLine + tab + "Promedio: ${2}\\.";
            var average = (exchangeRate.Rate.Buy + exchangeRate.Rate.Sell) / 2;

            var rate1 = string.Format(exchangeTemplate, exchangeRate.Rate.Buy, exchangeRate.Rate.Sell, average, exchangeRate.ProviderDescription);
            return header + fecha + newLine + rate1 + newLine;
        }
    }
}
