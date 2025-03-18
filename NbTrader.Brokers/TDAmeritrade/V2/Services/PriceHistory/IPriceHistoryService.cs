using NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory
{
    public interface IPriceHistoryService
    {
         Task<CandleList> GetPriceHistoryAsync(
             string symbol,
             int period,
             PeriodType periodType,
             int frequency,
             FrequencyType frequencyType,
             bool needExtendedHoursData);

        Task<CandleList> GetPriceHistoryAsync(
             string symbol,
             DateTime startDate,
             DateTime endDate,
             int frequency,
             FrequencyType frequencyType,
             bool needExtendedHoursData);
    }
}