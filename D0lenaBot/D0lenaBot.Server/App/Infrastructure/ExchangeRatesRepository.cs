using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using Microsoft.Azure.Cosmos;
using System;
using System.Net;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Infrastructure
{
    // ToDo: 
    // * Move instance variables to env files
    // * Create a proper instance of the cosmosClient
    // * Improve how the database and containers are created
    // * Think for one second about a proper PartitionKey
    // * Do some loging

    internal class ExchangeRatesRepository : IExchangeRates
    {
        private string EndpointUrl = "https://localhost:8081";
        private string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        private string databaseId = "DollarExchangeRate";
        private string containerId = "ExchangeRateContainer";
        public ExchangeRatesRepository()
        {
            this.cosmosClient = new CosmosClient(this.EndpointUrl, this.PrimaryKey);

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
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Item in database with id: {0} already exists\n", exchangeRate.Id);
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
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
    }
}
