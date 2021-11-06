using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchDolarSiExchangeRateCommand
{
    public interface IFetchDolarSiExchangeRateCommand
    {
        Task FetchTodaysExchangeRate();
    }
}
