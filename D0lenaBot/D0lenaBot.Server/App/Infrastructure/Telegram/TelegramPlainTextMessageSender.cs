using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure.Telegram
{
    // ToDo: 
    // Create unit test
    internal class TelegramPlainTextMessageSender : IPlainTextMessageSender
    {
        private const string PATH = "/sendMessage";
        private const string PARAM_NAME_TEXT = "text";
        private const string PARAM_NAME_CHAT_ID = "chat_id";
        private const string PARAM_NAME_PARSE_MODE = "parse_mode";
        private const string PARAM_VALUE_PARSE_MODE = "MarkdownV2";

        private readonly ITelegramMessageSender telegramMessageSender;

        public TelegramPlainTextMessageSender(ITelegramMessageSender telegramMessageSender)
        {
            this.telegramMessageSender = telegramMessageSender;
        }
        public async Task Send(string message, string chatId)
        {
            var text = this.GetText(message);

            var args = new Dictionary<string, string>();
            args.Add(PARAM_NAME_TEXT, text);
            args.Add(PARAM_NAME_CHAT_ID, chatId);
            args.Add(PARAM_NAME_PARSE_MODE, PARAM_VALUE_PARSE_MODE);

            await this.telegramMessageSender.SendMessage(PATH, args);
        }

        private string GetText(string message)
        {
            var messageBuilder = new TelegramMessageBuilder();

            return messageBuilder.AddText(message)
                                 .ToString();
        }
    }
}
