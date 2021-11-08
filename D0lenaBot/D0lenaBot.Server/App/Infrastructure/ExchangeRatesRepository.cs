using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure
{
    // ToDo: 
    // * Create a proper instance of the cosmosClient
    // * Improve how the database and containers are created
    // * Think for one second about a proper PartitionKey
    // * Do some loging
    // * Improve how we read data from container
    internal class ExchangeRatesRepository : IExchangeRates
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        private readonly string databaseId;
        private readonly string containerId;
        public ExchangeRatesRepository(IEnvironmentVariablesProvider environmentVariablesProvider)
        {
            this.databaseId = environmentVariablesProvider.GetDatabaseId();
            this.containerId = environmentVariablesProvider.GetContainerId();

            var endpointUrl = environmentVariablesProvider.GetDatabaseEndpointUrl();
            var primaryKey = environmentVariablesProvider.GetDatabasePrimaryKey();
            this.cosmosClient = new CosmosClient(endpointUrl, primaryKey);

        }
        public async Task Save(ExchangeRate exchangeRate)
        {
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();

            try
            {
                ItemResponse<ExchangeRate> exchangeRateResponse = await this.container.CreateItemAsync<ExchangeRate>(exchangeRate, new PartitionKey(exchangeRate.ProviderDescription));

                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", exchangeRateResponse.Resource.Id, exchangeRateResponse.RequestCharge);
            }
            catch (CosmosException cosmosEx) when (cosmosEx.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Item in database with id: {0} already exists\n", exchangeRate.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("saved");
        }

        private async Task CreateDatabaseAsync()
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        private async Task CreateContainerAsync()
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(this.containerId, "/ProviderDescription");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        public async Task<ExchangeRate> GetLatest()
        {
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();

            try
            {
                // where c.CreatedDateUTC <= '{date.ToString("yyyy-MM-ddThh:mm:ss.fffZ")}'
                var sqlQueryText = $"SELECT * FROM c order by c.CreatedDateUTC desc OFFSET 0 LIMIT 1";

                Console.WriteLine("Running query: {0}\n", sqlQueryText);

                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<ExchangeRate> queryResultSetIterator = this.container.GetItemQueryIterator<ExchangeRate>(queryDefinition);

                List<ExchangeRate> families = new List<ExchangeRate>();

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<ExchangeRate> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (ExchangeRate family in currentResultSet)
                    {
                        return family;
                    }
                }
                throw new Exception("No more results");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
