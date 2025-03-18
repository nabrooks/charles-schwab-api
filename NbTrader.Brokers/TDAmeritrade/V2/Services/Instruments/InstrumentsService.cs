using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments
{

    public class InstrumentsService : AbstractService, IInstrumentsService
    {
        public InstrumentsService(
            IHttp httpClient,
            IHttpRequestMessageService httpRequestMessageService)
                : base(httpClient, httpRequestMessageService)
        {
        }

        public async Task<Fundamental> GetFundamentalAsync(string symbol)
        {
            IDictionary<string,Fundamental> response = await SendServiceCall<IDictionary<string,Fundamental>>(HttpMethod.Get, $"/instruments?symbol={symbol}&projection=fundamental");

            if(response.Values.Count>0)
                return response[symbol];
            else
                return new Fundamental();
        }

        public async Task<IList<Instrument>> SearchAsync(string searchString, Projection projection)
        {
            //return await SendServiceCallWithEmbeddedAPIkey<IList<InstrumentTop>>(HttpMethod.Get, $"/instruments?symbol={searchString}&projection={ProjectionToString.Value(projection)}");

            IDictionary<string,Instrument> response = await SendServiceCall<IDictionary<string,Instrument>>(HttpMethod.Get, $"/instruments?symbol={searchString}&projection={ProjectionToString.Value(projection)}");

            IList<Instrument> instrumentList = new List<Instrument>();
            foreach(var v in response.Values) instrumentList.Add(v);
            //return Shared.Utilities.JsonConfig.DeserializeObject<IList<Instrument>>(response);
            return instrumentList;
        }

        public async Task<Instrument> GetInstrumentByCUSIPAsync(string cusip)
        {
            IList<Instrument> result = await GetInstrumentByCUSIPasListAsync(cusip);
            if(result.Count == 1) return result[0];
            else return new Instrument{Symbol = cusip + "_NOT_FOUND"};
        }

        protected async Task<IList<Instrument>> GetInstrumentByCUSIPasListAsync(string cusip)
        {
            return await SendServiceCall<IList<Instrument>>(HttpMethod.Get, $"/instruments/{cusip}?");
        }
    }
}