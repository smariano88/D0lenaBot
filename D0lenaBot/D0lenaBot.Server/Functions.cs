using D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand;
using D0lenaBot.Server.App.Application.NotifyExchangeRateCommand;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
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
    public class FetchDollarExchangeRates
    {
        private readonly IFetchDolarSiExchangeRateCommand fetchDollarCommand;
        private readonly INotifyExchangeRateCommand notifyExchangeRateCommand;
        private readonly IRegisterUserCommand registerUserCommand;
        private readonly ISendWelcomeMessageCommand sendWelcomeMessageCommand;

        public FetchDollarExchangeRates(
            IFetchDolarSiExchangeRateCommand fetchDollarCommand,
            INotifyExchangeRateCommand notifyExchangeRateCommand,
            IRegisterUserCommand registerUserCommand,
            ISendWelcomeMessageCommand sendWelcomeMessageCommand)
        {
            this.fetchDollarCommand = fetchDollarCommand;
            this.notifyExchangeRateCommand = notifyExchangeRateCommand;
            this.registerUserCommand = registerUserCommand;
            this.sendWelcomeMessageCommand = sendWelcomeMessageCommand;
        }

        [FunctionName("FetchDolarSiTodaysExchangeRate")]
        public async Task Run([TimerTrigger("0 30 14 * * 1-5")] TimerInfo myTimer, ILogger logger)
        {
            try
            {
                await this.fetchDollarCommand.FetchTodaysExchangeRate();
                await this.notifyExchangeRateCommand.Send(DateTime.UtcNow);
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

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string textMessage = data?.message?.text?.ToString();

            if (string.IsNullOrEmpty(textMessage))
            {
                log.LogError("Body: " + requestBody);
                return (ActionResult)new OkObjectResult("ok");
            }

            switch (textMessage)
            {
                case "/start":
                    {
                        string chatId = data.message.chat.id.ToString();
                        await this.sendWelcomeMessageCommand.Send(chatId);
                        break;
                    }
                case "/subscribe":
                    {
                        string chatId = data.message.chat.id.ToString();
                        await this.registerUserCommand.Register(chatId);
                        break;
                    }
                case "/stop":
                    {
                        //string chatId = data.message.chat.id.ToString();
                        //await this.registerUserCommand.Register(chatId);
                        break;
                    }
                case "/diaria":
                    {
                        //string chatId = data.message.chat.id.ToString();
                        //await this.registerUserCommand.Register(chatId);
                        break;
                    }
                default:
                    {
                        log.LogError("Body: " + requestBody);
                        break;
                    }
            }

            return (ActionResult)new OkObjectResult("ok");
        }
    }
}
