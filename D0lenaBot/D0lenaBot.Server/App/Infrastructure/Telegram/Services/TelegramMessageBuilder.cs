using System.Collections.Generic;
using System.Text;

namespace D0lenaBot.Server.App.Infrastructure.Telegram.Services
{
    internal interface ITelegramMessageBuilder
    {
        ITelegramMessageBuilder AddText(string text);
        ITelegramMessageBuilder AddBoldText(string text);
        ITelegramMessageBuilder AddItalicText(string text);
        ITelegramMessageBuilder AddNewLine();
        ITelegramMessageBuilder AddBullet();
        string ToString();
    }

    internal class TelegramMessageBuilder : ITelegramMessageBuilder
    {
        private const string TELEGRAM_NEW_LINE = "%0A";

        private StringBuilder stringBuilder = new StringBuilder();
        public ITelegramMessageBuilder AddBoldText(string text)
        {
            this.stringBuilder.Append("*");
            this.AddText(text);
            this.stringBuilder.Append("*");
            return this;
        }

        public ITelegramMessageBuilder AddItalicText(string text)
        {
            this.stringBuilder.Append("_");
            this.AddText(text);
            this.stringBuilder.Append("_");
            return this;
        }

        public ITelegramMessageBuilder AddNewLine()
        {
            this.stringBuilder.Append(TELEGRAM_NEW_LINE);
            return this;
        }

        public ITelegramMessageBuilder AddText(string text)
        {
            var sanatizedText = this.SanitizeText(text);
            this.stringBuilder.Append(sanatizedText);
            return this;
        }

        public ITelegramMessageBuilder AddBullet()
        {
            this.stringBuilder.Append("● ");//⚫
            return this;
        }

        private static IEnumerable<string> CharsToBeSanitized = new List<string>()
        {
            ".", "!", "(", ")"
        };

        private string SanitizeText(string text)
        {
            var sanatizedText = text;
            foreach (var charToBeReplaced in CharsToBeSanitized)
            {
                sanatizedText = sanatizedText.Replace(charToBeReplaced, $"\\{charToBeReplaced}");
            }
            return sanatizedText;
        }

        public override string ToString() { return this.stringBuilder.ToString(); }
    }
}
