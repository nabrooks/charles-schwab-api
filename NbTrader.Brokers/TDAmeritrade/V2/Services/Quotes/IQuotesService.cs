using NbTrader.Brokers.TDAmeritrade.V2.Services.Quotes.Models;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Quotes
{
    public interface IQuotesService
    {
         Task<Equity> GetEquityQuoteAsync(string symbol);
         Task<IList<Equity>> GetEquityQuotesAsync(string[] symbol);
    }
}