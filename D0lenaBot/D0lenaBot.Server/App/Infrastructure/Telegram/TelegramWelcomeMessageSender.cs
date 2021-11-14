using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure.Telegram
{
    internal class TelegramWelcomeMessageSender : IWelcomeMessageSender
    {
        private const string PATH = "/sendMessage";
        private const string PARAM_NAME_TEXT = "text";
        private const string PARAM_NAME_CHAT_ID = "chat_id";
        private const string PARAM_NAME_PARSE_MODE = "parse_mode";
        private const string PARAM_VALUE_PARSE_MODE = "MarkdownV2";

        private readonly ITelegramMessageSender telegramMessageSender;
dw
        public TelegramWelcomeMessageSender(ITelegramMessageSender telegramMessageSender)
        {
            this.telegramMessageSender = telegramMessageSender;
        }

        public async Task Send(string chatId)
        {
            var text = this.GetText();

            var args = new Dictionary<string, string>();
            args.Add(PARAM_NAME_TEXT, text);
            args.Add(PARAM_NAME_CHAT_ID, chatId);
            args.Add(PARAM_NAME_PARSE_MODE, PARAM_VALUE_PARSE_MODE);

            await this.telegramMessageSender.SendMessage(PATH, args);
        }

        private string GetText()
        {
            var messageBuilder = new TelegramMessageBuilder();
            messageBuilder.AddText("Bienvenido! Mi nombre es ").AddBoldText("D0lenaBot").AddText(" y estoy acá para ayudarte con la cotización del dólar blue en ").AddBoldText("Rosario").AddText(".").AddNewLine();
            
            messageBuilder.AddNewLine().AddText("Para empezar, mandá alguno de los siguientes comandos:");

            messageBuilder.AddNewLine().AddBullet().AddText("/subscribe para empezar a recibir la cotización de lunes a viernes a las 11:30am");
            messageBuilder.AddNewLine().AddBullet().AddText("/stop para cancelar la suscripción");
            messageBuilder.AddNewLine().AddBullet().AddText("/coti para recibir la última cotización disponible. Útil cuando no te interesa recibir la cotización todos los días y preferís que te la mandemos cuando vos la necesitás");

            return messageBuilder.ToString();
        }
    }
}
