using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using System.Collections.Generic;
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
        private readonly ITelegramMessageBuilder telegramMessageBuilder;

        public TelegramExchangeRateMessageSender(ITelegramMessageSender telegramMessageSender, ITelegramMessageBuilder telegramMessageBuilder)
        {
            this.telegramMessageSender = telegramMessageSender;
            this.telegramMessageBuilder = telegramMessageBuilder;
        }
        public async Task SendExchangeRate(ExchangeRate exchangeRate, string chatId)
        {
            var text = this.GetText(exchangeRate);

            var args = new Dictionary<string, string>();
            args.Add(PARAM_NAME_TEXT, text);
            args.Add(PARAM_NAME_CHAT_ID, chatId);
            args.Add(PARAM_NAME_PARSE_MODE, PARAM_VALUE_PARSE_MODE);

            await this.telegramMessageSender.SendMessage(PATH, args);
        }

        private string GetText(ExchangeRate exchangeRate)
        {
            string exchangeRateText = string.Format("${0} / ${1}\\. ", exchangeRate.Rate.Buy.ToString(), exchangeRate.Rate.Sell.ToString());
            string average = string.Format(" Promedio: ${0}", exchangeRate.Rate.Average);

            var messageBuilder = this.telegramMessageBuilder
                                     .AddItalicText("Fecha: ").AddText(exchangeRate.ExchangeDateUTC.ToString("dd/MM/yyyy"))
                                     .AddNewLine().AddNewLine().AddText("💵 ").AddText(exchangeRate.ProviderDescription)
                                     .AddNewLine().AddBoldText(exchangeRateText).AddText(average);

            /*var messageBuilder = this.telegramMessageBuilder
                                     .AddText("💵 ").AddBoldText(exchangeRate.ProviderDescription)
                                     .AddNewLine().AddItalicText("Fecha coti: ").AddText(exchangeRate.ExchangeDateUTC.ToString("dd/MM/yyyy"))
                                     .AddNewLine().AddText(exchangeRateText).AddText(average);*/

            return messageBuilder.ToString();
        }
    }
}
