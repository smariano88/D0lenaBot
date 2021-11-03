using D0lenaBot.Server.App.Application.FetchDollarCommand;
using D0lenaBot.Server.App.Application.NotifyExchangeRateCommand;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using D0lenaBot.Server.App.Application.RegisterUserCommand;

namespace D0lenaBot.Server
{
    public class FetchDollarExchangeRates
    {
        private readonly IFetchDollarCommand fetchDollarCommand;
        private readonly INotifyExchangeRateCommand notifyExchangeRateCommand;
        private readonly IRegisterUserCommand registerUserCommand;

        public FetchDollarExchangeRates(
            IFetchDollarCommand fetchDollarCommand,
            INotifyExchangeRateCommand notifyExchangeRateCommand,
            IRegisterUserCommand registerUserCommand)
        {
            this.fetchDollarCommand = fetchDollarCommand;
            this.notifyExchangeRateCommand = notifyExchangeRateCommand;
            this.registerUserCommand = registerUserCommand;
        }

        [FunctionName("FetchDollarExchangeRate")]
        public async Task Run([TimerTrigger("0 30 13 * * *")] TimerInfo myTimer, ILogger logger)
        {
            try
            {
                await this.fetchDollarCommand.Fetch(DateTime.UtcNow);
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
            string textMessage = data.message.text.ToString();

            if (textMessage == "/start")
            {
                string chatId = data.message.chat.id.ToString();
                await this.registerUserCommand.Register(chatId);
            }

            return (ActionResult)new OkObjectResult("ok");
        }
    }
}
