using D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand;
using D0lenaBot.Server.App.Application.NotifyAllExchangeRateCommand;
using D0lenaBot.Server.App.Application.NotifyExchangeRateToOnePersonCommand;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
using D0lenaBot.Server.App.Application.RemoveUserCommand;
using D0lenaBot.Server.App.Application.SendWelcomeMessageCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace D0lenaBot.Server
{
    // ToDo: 
    // * SRP
    // * Add mediator
    // * Add constants to messages (or use some sort of factory)
    public class FetchDollarExchangeRates
    {
        private readonly IFetchDolarSiExchangeRateCommand fetchDollarCommand;
        private readonly INotifyAllExchangeRateCommand notifyExchangeRateCommand;
        private readonly IRegisterUserCommand registerUserCommand;
        private readonly ISendWelcomeMessageCommand sendWelcomeMessageCommand;
        private readonly IRemoveUserCommand removeUserCommand;
        private readonly INotifyExchangeRateToOnePersonCommand notifyExchangeRateToOnePersonCommand;

        public FetchDollarExchangeRates(
            IFetchDolarSiExchangeRateCommand fetchDollarCommand,
            INotifyAllExchangeRateCommand notifyExchangeRateCommand,
            IRegisterUserCommand registerUserCommand,
            ISendWelcomeMessageCommand sendWelcomeMessageCommand,
            IRemoveUserCommand removeUserCommand,
            INotifyExchangeRateToOnePersonCommand notifyExchangeRateToOnePersonCommand)
        {
            this.fetchDollarCommand = fetchDollarCommand;
            this.notifyExchangeRateCommand = notifyExchangeRateCommand;
            this.registerUserCommand = registerUserCommand;
            this.sendWelcomeMessageCommand = sendWelcomeMessageCommand;
            this.removeUserCommand = removeUserCommand;
            this.notifyExchangeRateToOnePersonCommand = notifyExchangeRateToOnePersonCommand;
        }

        [FunctionName("FetchDolarSiTodaysExchangeRate")]
        public async Task Run([TimerTrigger("0 30 14 * * 1-5")] TimerInfo myTimer, ILogger logger)
        {
            try
            {
                await this.fetchDollarCommand.FetchTodaysExchangeRate();
                await this.notifyExchangeRateCommand.Send();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [FunctionName("webhook")]
        public async Task<IActionResult> RunRegister([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            try
            {
                await this.SendMessage(requestBody, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Unexpected error");
            }

            return (ActionResult)new OkObjectResult("ok");
        }

        private async Task SendMessage(string requestBody, ILogger log)
        {
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string textMessage = data?.message?.text?.ToString();
            string chatId = data?.message?.chat?.id.ToString();
            if (string.IsNullOrEmpty(textMessage)
                || string.IsNullOrEmpty(chatId))
            {
                log.LogError("Body: " + requestBody);
                return;
            }

            switch (textMessage)
            {
                case "/start":
                    await this.sendWelcomeMessageCommand.Send(chatId);
                    break;
                case "/subscribe":
                    string firstName = data.message.chat.first_name.ToString();
                    string lastName = data.message.chat.last_name.ToString();
                    await this.registerUserCommand.Register(chatId, firstName, lastName);
                    break;
                case "/stop":
                    await this.removeUserCommand.Remove(chatId);
                    break;
                case "/coti":
                    await this.notifyExchangeRateToOnePersonCommand.Send(chatId);
                    break;
                default:
                    log.LogError("Body: " + requestBody);
                    break;
            }
        }
    }
}
