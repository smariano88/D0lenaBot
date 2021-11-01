using System;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchDollarQuery
{
    public interface IFetchDollarQuery
    {
        Task Fetch(DateTime date);
    }
}
