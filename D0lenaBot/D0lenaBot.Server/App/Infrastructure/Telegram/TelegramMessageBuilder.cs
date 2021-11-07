using System.Text;

namespace D0lenaBot.Server.App.Infrastructure.Telegram
{
    internal interface ITelegramMessageBuilder
    {
        ITelegramMessageBuilder AddText(string text);
        ITelegramMessageBuilder AddBoldText(string text);
        ITelegramMessageBuilder AddItalicText(string text);
        ITelegramMessageBuilder AddNewLine();
        string ToString();
    }

    internal class TelegramMessageBuilder : ITelegramMessageBuilder
    {
        private const string TELEGRAM_NEW_LINE = "%0A";

        private StringBuilder stringBuilder = new StringBuilder();
        public ITelegramMessageBuilder AddBoldText(string text)
        {
            this.stringBuilder.Append($"*{text}*");
            return this;
        }

        public ITelegramMessageBuilder AddItalicText(string text)
        {
            this.stringBuilder.Append($"_{text}_");
            return this;
        }

        public ITelegramMessageBuilder AddNewLine()
        {
            this.stringBuilder.Append(TELEGRAM_NEW_LINE);
            return this;
        }

        public ITelegramMessageBuilder AddText(string text)
        {
            this.stringBuilder.Append(text);
            return this;
        }

        public override string ToString() { return this.stringBuilder.ToString(); }
    }
}
