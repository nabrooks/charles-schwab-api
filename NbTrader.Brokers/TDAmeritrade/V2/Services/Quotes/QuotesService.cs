using System.Net;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Quotes.Models;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Quotes
{

    public class QuotesService : AbstractService, IQuotesService
    {
        public QuotesService(
            IHttp httpClient,
            IHttpRequestMessageService httpRequestMessageService)
                : base(httpClient, httpRequestMessageService)
        {
        }

        public async Task<Equity> GetEquityQuoteAsync(string symbol)
        {
            IDictionary<string, Equity> response = await SendServiceCall<IDictionary<string, Equity>>(HttpMethod.Get, $"/marketdata/{symbol.ToUpper()}/quotes");

            // response = JsonExtensions.parsedSearchResponse(response);
            // response = response.Remove(0, 1);
            // response = response.Remove(response.Length - 1, 1);

            if(response.Values.Count > 0)
            {
                return response[symbol.ToUpper()];
            }
            else
            {
                return new Equity();
            }

            //return Shared.Utilities.JsonConfig.DeserializeObject<Equity>(response);
        }

        public async Task<IList<Equity>> GetEquityQuotesAsync(string[] symbols)
        {
            string commaSeparatedSymbols = symbols[0].ToUpper();
            for(int i =1; i<symbols.Length; i++)
            {
                commaSeparatedSymbols += "," + symbols[i].ToUpper();
            }

            IDictionary<string,Equity> response = await SendServiceCall<IDictionary<string,Equity>>(HttpMethod.Get, $"/marketdata/quotes?symbol={WebUtility.UrlEncode(commaSeparatedSymbols)}");

            //convert to array
            //response = "[" + response.Substring(1,response.Length - 2) + "]";
            //response = JsonExtensions.parsedSearchResponse(response);

            //Console.WriteLine(response);

            //return Shared.Utilities.JsonConfig.DeserializeObject<IList<Equity>>(response);

            IList<Equity> result = new List<Equity>();
            foreach(var e in response.Values)
            {
                result.Add(e);
            }
            return result;
        }
    }
}