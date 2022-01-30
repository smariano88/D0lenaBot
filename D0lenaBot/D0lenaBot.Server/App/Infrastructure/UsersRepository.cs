using D0lenaBot.Server.App.Application.Infrastructure;
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
    internal class UsersRepository : IUsers
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private const string PartitionKey = "/" + Domain.User.CityPropertyName;
        private const string PartitionValue = "Rosario";

        private readonly string databaseId;
        private readonly string containerId;
        public UsersRepository(IEnvironmentVariablesProvider environmentVariablesProvider)
        {
            this.databaseId = environmentVariablesProvider.GetDatabaseId();
            this.containerId = environmentVariablesProvider.GetUsersContainerId();

            var endpointUrl = environmentVariablesProvider.GetDatabaseEndpointUrl();
            var primaryKey = environmentVariablesProvider.GetDatabasePrimaryKey();
            this.cosmosClient = new CosmosClient(endpointUrl, primaryKey);
        }

        public async Task Save(Domain.User user)
        {
            await EnsureCreated();

            try
            {
                ItemResponse<Domain.User> exchangeRateResponse = await this.container.CreateItemAsync(user, new PartitionKey(user.City));

                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", exchangeRateResponse.Resource.Id, exchangeRateResponse.RequestCharge);
            }
            catch (CosmosException cosmosEx) when (cosmosEx.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Item in database with id: {0} already exists\n", user.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            Console.WriteLine("saved");
        }

        public async Task<IEnumerable<Domain.User>> GetAll()
        {
            await this.EnsureCreated();

            try
            {
                return this.container.GetItemLinqQueryable<Domain.User>(true)
                                     .AsEnumerable()
                                     .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Domain.User> GetByChatId(string chatId)
        {
            await this.EnsureCreated();

            try
            {
                var response = await this.container.ReadItemAsync<Domain.User>(chatId, new PartitionKey(PartitionValue));
                Console.WriteLine("User read. Operation consumed {0} RUs.\n", response.RequestCharge);

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Delete(Domain.User user)
        {
            await EnsureCreated();

            try
            {
                var response = await this.container.DeleteItemAsync<Domain.User>(user.Id, new PartitionKey(user.City));

                Console.WriteLine("Deleted item in database with id: {0} Operation consumed {1} RUs.\n", user.Id, response.RequestCharge);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            Console.WriteLine("removed");
        }

        private async Task EnsureCreated()
        {
            if (this.container != null)
            {
                Console.WriteLine("Databases already created");
                return;
            }
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);

            this.container = await this.database.CreateContainerIfNotExistsAsync(this.containerId, PartitionKey);
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }
    }
}
