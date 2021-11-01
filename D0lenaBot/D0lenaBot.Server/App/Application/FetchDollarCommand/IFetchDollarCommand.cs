using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchDollarCommand
{
    public interface IFetchDollarCommand
    {
        Task Fetch(DateTime date);
    }
}
