using System;
using System.Collections.Generic;
using System.Text;

namespace D0lenaBot.Server.App.Infrastructure
{
    public interface IEnvironmentVariablesProvider
    {
        string GetTelegramToken();
        string GetDatabaseEndpointUrl();
        string GetDatabasePrimaryKey();
        string GetDatabaseId();
        string GetContainerId();
    }
}
