using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D0lenaBot.Server.App.Application.FetchDollarQuery
{
    internal class FetchDollarQuery : IFetchDollarQuery
    {
        private readonly ILogger logger;
        public FetchDollarQuery(ILogger<FetchDollarQuery> logger)
        {
            this.logger = logger;
        }

        public void Fetch(DateTime date)
        {
            this.logger.LogError($"hello from logs {date}");
        }
    }
}
