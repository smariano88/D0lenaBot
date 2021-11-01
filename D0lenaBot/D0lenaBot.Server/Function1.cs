using D0lenaBot.Server.App.Application.FetchDollarQuery;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server
{
    public class FetchDollarExchangeRates
    {
        private readonly IFetchDollarQuery fetchDollarQuery;

        public FetchDollarExchangeRates(IFetchDollarQuery fetchDollarQuery)
        {
            this.fetchDollarQuery = fetchDollarQuery;
        }

        [FunctionName("FetchDollarExchangeRate")]
        public async Task Run([TimerTrigger("0 30 10 * * *")] TimerInfo myTimer, ILogger logger)
        {
            try
            {
                await this.fetchDollarQuery.Fetch(DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
