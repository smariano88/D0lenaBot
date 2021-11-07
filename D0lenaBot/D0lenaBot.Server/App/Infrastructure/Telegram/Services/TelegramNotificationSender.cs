using D0lenaBot.Server.App.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure.Telegram.Services
{
    // ToDo: improve use of httpclient
    internal interface ITelegramMessageSender
    {
        Task SendMessage(string path, Dictionary<string, string> queryArgs);
    }
    internal class TelegramMessageSender : ITelegramMessageSender

    {
        public const string BASE_URL = "https://api.telegram.org/bot";
        public readonly string telegramToken;

        public TelegramMessageSender(IEnvironmentVariablesProvider environmentVariablesProvider)
        {
            this.telegramToken = environmentVariablesProvider.GetTelegramToken();
        }

        public async Task SendMessage(string path, Dictionary<string, string> queryArgs)
        {
            HttpClient req = new HttpClient();
            string finalUrl = $"{BASE_URL}{telegramToken}{path}?{queryArgs.ToQueryStringArgs()}";
            var response = await req.GetAsync(finalUrl);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var contents = response.Content.ReadAsStringAsync().Result;
                // "{\"ok\":false,\"error_code\":400,\"description\":\"Bad Request: chat not found\"}"
            }
        }
    }
}
