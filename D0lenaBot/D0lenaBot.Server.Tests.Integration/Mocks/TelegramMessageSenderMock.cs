using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration.Mocks
{
    public class TelegramMessageSenderMock : ITelegramMessageSender
    {
        public List<Message> Messages { get; set; } = new List<Message>();
        public async Task SendMessage(string path, Dictionary<string, string> queryArgs)
        {
            this.Messages.Add(new Message(path, queryArgs));
        }
    }
    public class Message
    {
        public Message(string path, Dictionary<string, string> queryArgs)
        {
            Path = path;
            QueryArgs = queryArgs;
        }

        public string Path { get; set; }
        public Dictionary<string, string> QueryArgs { get; set; }
    }
}
