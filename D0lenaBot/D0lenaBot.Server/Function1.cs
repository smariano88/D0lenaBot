using D0lenaBot.Server.App.Application.FetchDollarCommand;
using D0lenaBot.Server.App.Application.NotifyExchangeRateCommand;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server
{
    public class FetchDollarExchangeRates
    {
        private readonly IFetchDollarCommand fetchDollarCommand;
        private readonly INotifyExchangeRateCommand notifyExchangeRateCommand;

        public FetchDollarExchangeRates(IFetchDollarCommand fetchDollarCommand, INotifyExchangeRateCommand notifyExchangeRateCommand)
        {
            this.fetchDollarCommand = fetchDollarCommand;
            this.notifyExchangeRateCommand = notifyExchangeRateCommand;
        }

        [FunctionName("FetchDollarExchangeRate")]
        public async Task Run([TimerTrigger("0 30 10 * * *")] TimerInfo myTimer, ILogger logger)
        {
            try
            {
                await this.fetchDollarCommand.Fetch(DateTime.Now);
                await this.notifyExchangeRateCommand.Send(DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
