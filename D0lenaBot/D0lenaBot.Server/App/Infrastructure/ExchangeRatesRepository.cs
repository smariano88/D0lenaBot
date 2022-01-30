using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
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
            await this.EnsureCreated();

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

        public async Task<ExchangeRate> GetLatest()
        {
            await this.EnsureCreated();

            try
            {
                return this.container.GetItemLinqQueryable<Domain.ExchangeRate>(true)
                    .OrderByDescending(u => u.ExchangeDateUTC)
                    .AsEnumerable()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task EnsureCreated()
        {
            if (this.container != null)
            {
                Console.WriteLine("Database already created");
                return;
            }

            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);

            this.container = await this.database.CreateContainerIfNotExistsAsync(this.containerId, "/ProviderDescription");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRateFor(DateTime date)
        {
            await this.EnsureCreated();

            try
            {
                return this.container.GetItemLinqQueryable<Domain.ExchangeRate>(true)
                    .Where(e => e.ExchangeDateUTC == date)
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
