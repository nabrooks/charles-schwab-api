using NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours
{
    public interface IMarketHoursService
    {
        //Task<MarketTypeHours> GetMarketHoursAsync(MarketType market, DateTime date);
        //Task<IList<Session>> GetMarketHoursAsync(DateTime date);

        Task<IList<Hours>> GetMarketHoursAsync(MarketType marketType, DateTime date);
    }
}