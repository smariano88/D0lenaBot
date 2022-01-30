using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.FetchCotizacionCoExchangeRateCommand
{
    public interface IFetchCotizacionCoExchangeRateCommand
    {
        Task FetchTodaysExchangeRate();
    }
}
