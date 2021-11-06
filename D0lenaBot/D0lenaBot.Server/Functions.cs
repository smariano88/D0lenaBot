using D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand;
using D0lenaBot.Server.App.Application.NotifyExchangeRateCommand;
using D0lenaBot.Server.App.Application.RegisterUserCommand;
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

        public FetchDollarExchangeRates(
            IFetchDolarSiExchangeRateCommand fetchDollarCommand,
            INotifyExchangeRateCommand notifyExchangeRateCommand,
            IRegisterUserCommand registerUserCommand)
        {
            this.fetchDollarCommand = fetchDollarCommand;
            this.notifyExchangeRateCommand = notifyExchangeRateCommand;
            this.registerUserCommand = registerUserCommand;
        }

        [FunctionName("FetchDolarSiTodaysExchangeRate")]
        public async Task Run([TimerTrigger("0 30 14 * * *")] TimerInfo myTimer, ILogger logger)
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

        [FunctionName("RegisterNewUser")]
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
            }
            else if (textMessage == "/start")
            {
                string chatId = data.message.chat.id.ToString();
                await this.registerUserCommand.Register(chatId);
            }
            else
            {
                log.LogError("Body: " + requestBody);
            }

            return (ActionResult)new OkObjectResult("ok");
        }
    }
}
