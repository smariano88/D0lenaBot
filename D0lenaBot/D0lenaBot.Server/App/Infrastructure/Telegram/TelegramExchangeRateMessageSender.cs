using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure.Telegram
{
    // ToDo: 
    // Create unit test
    internal class TelegramExchangeRateMessageSender : IExchangeRateMessageSender
    {
        private const string PATH = "/sendMessage";
        private const string PARAM_NAME_TEXT = "text";
        private const string PARAM_NAME_CHAT_ID = "chat_id";
        private const string PARAM_NAME_PARSE_MODE = "parse_mode";
        private const string PARAM_VALUE_PARSE_MODE = "MarkdownV2";

        private readonly ITelegramMessageSender telegramMessageSender;

        public TelegramExchangeRateMessageSender(ITelegramMessageSender telegramMessageSender)
        {
            this.telegramMessageSender = telegramMessageSender;
        }
        public async Task SendExchangeRate(IEnumerable<ExchangeRate> exchangeRates, string chatId)
        {
            var text = this.GetText(exchangeRates);

            var args = new Dictionary<string, string>();
            args.Add(PARAM_NAME_TEXT, text);
            args.Add(PARAM_NAME_CHAT_ID, chatId);
            args.Add(PARAM_NAME_PARSE_MODE, PARAM_VALUE_PARSE_MODE);

            await this.telegramMessageSender.SendMessage(PATH, args);
        }

        private string GetText(IEnumerable<ExchangeRate> exchangeRates)
        {
            var messageBuilder = new TelegramMessageBuilder();
            if (exchangeRates.Count() == 0)
            {
                messageBuilder.AddNewLine().AddText("Todavia no hay cotización. Proba nuevamente más tarde");
                return messageBuilder.ToString();
            }

            messageBuilder.AddItalicText("Fecha: ").AddText(exchangeRates.First().ExchangeDateUTC.ToString("dd/MM/yyyy"));

            foreach (var exchangeRate in exchangeRates)
            {
                string exchangeRateText = string.Format("${0} / ${1}. ", exchangeRate.Rate.Buy.ToString(), exchangeRate.Rate.Sell.ToString());
                string average = string.Format(" Promedio: ${0}", exchangeRate.Rate.Average);

                messageBuilder.AddNewLine().AddNewLine().AddText("💵 ").AddText(exchangeRate.ProviderDescription);
                messageBuilder.AddNewLine().AddBoldText(exchangeRateText).AddText(average);
            }

            return messageBuilder.ToString();
        }
    }
}
