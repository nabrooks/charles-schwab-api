using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments
{
    public interface IInstrumentsService
    {
         Task<Fundamental> GetFundamentalAsync(string symbol);
         Task<IList<Instrument>> SearchAsync(string searchString, Projection projection);
         Task<Instrument> GetInstrumentByCUSIPAsync(string cusip);
    }
}