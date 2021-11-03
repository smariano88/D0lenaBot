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
    internal class UsersRepository : IUsers
    {
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

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
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();

            try
            {
                ItemResponse<Domain.User> exchangeRateResponse = await this.container.CreateItemAsync<Domain.User>(user, new PartitionKey(user.PartitionKey));

                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", exchangeRateResponse.Resource.Id, exchangeRateResponse.RequestCharge);
            }
            catch (CosmosException cosmosEx) when (cosmosEx.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Item in database with id: {0} already exists\n", user.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            Console.WriteLine("saved");
        }

        public async Task<IEnumerable<Domain.User>> GetAll()
        {
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();

            try
            {
                var sqlQueryText = $"SELECT * FROM c";

                Console.WriteLine("Running query: {0}\n", sqlQueryText);

                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<Domain.User> queryResultSetIterator = this.container.GetItemQueryIterator<Domain.User>(queryDefinition);

                List<Domain.User> users = new List<Domain.User>();
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Domain.User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Domain.User family in currentResultSet)
                    {
                        users.Add(family);
                    }
                }

                return users;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public async Task<Domain.User> GetByChatId(string chatId)
        {
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();

            try
            {
                // where c.CreatedDateUTC <= '{date.ToString("yyyy-MM-ddThh:mm:ss.fffZ")}'
                var sqlQueryText = $"SELECT * FROM c WHERE c.ChatId = '{chatId}'";

                Console.WriteLine("Running query: {0}\n", sqlQueryText);

                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<Domain.User> queryResultSetIterator = this.container.GetItemQueryIterator<Domain.User>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Domain.User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Domain.User family in currentResultSet)
                    {
                        return family;
                    }
                }

                return null;
            }

            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private async Task CreateDatabaseAsync()
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        private async Task CreateContainerAsync()
        {
            this.container = await this.database.CreateContainerIfNotExistsAsync(this.containerId, "/PartitionKey");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }
    }
}
