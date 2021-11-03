using D0lenaBot.Server.App.Infrastructure;
using System;

namespace D0lenaBot.Server
{
    internal class EnvironmentVariablesProvider : IEnvironmentVariablesProvider
    {
        private const string TELEGRAM_TOKEN = "Telegram_token";
        private const string COSMOSDB_ENDPOINTURL = "CosmosDB_EndpointUrl";
        private const string COSMOSDB_PRIMARYKEY = "CosmosDB_PrimaryKey";
        private const string COSMOSDB_DATABASEID = "CosmosDB_DatabaseId";
        private const string COSMOSDB_CONTAINERID = "CosmosDB_ContainerId";
        private const string COSMOSDB_USERSCONTAINERID = "CosmosDB_UsersContainerId";

        public string GetTelegramToken()
        {
            return Environment.GetEnvironmentVariable(TELEGRAM_TOKEN);
        }

        public string GetDatabaseEndpointUrl()
        {
            return Environment.GetEnvironmentVariable(COSMOSDB_ENDPOINTURL);
        }

        public string GetDatabasePrimaryKey()
        {
            return Environment.GetEnvironmentVariable(COSMOSDB_PRIMARYKEY);
        }

        public string GetDatabaseId()
        {
            return Environment.GetEnvironmentVariable(COSMOSDB_DATABASEID);
        }

        public string GetContainerId()
        {
            return Environment.GetEnvironmentVariable(COSMOSDB_CONTAINERID);
        }

        public string GetUsersContainerId()
        {
            return Environment.GetEnvironmentVariable(COSMOSDB_USERSCONTAINERID);
        }
    }
}
