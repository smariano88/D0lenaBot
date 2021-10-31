using D0lenaBot.Server.App.Application.FetchDollarQuery;
using Microsoft.Azure.WebJobs;
using System;

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
        public void Run([TimerTrigger("0 30 10 * * *")] TimerInfo myTimer)
        {
            this.fetchDollarQuery.Fetch(DateTime.Now);
        }
    }
}
