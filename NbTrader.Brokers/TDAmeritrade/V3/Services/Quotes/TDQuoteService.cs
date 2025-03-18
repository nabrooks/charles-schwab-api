using NbTrader.Brokers.TDAmeritrade.Models;
using NbTrader.Utility;
using System.Net;
using System.Text.Json;
using System.Web;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes
{
    public class TDQuoteService : AbstractService
    {
        public TDQuoteService(IHttp http, IHttpRequestMessageService httpRequestMessageService)
            : base(http, httpRequestMessageService)
        { }

        public Task<TDQuote> GetQuote(string symbol, TDAssetType assetType)
        {
            return assetType switch
            {
                TDAssetType.Equity => GetQuote<TDEquityQuote>(symbol),
                TDAssetType.EquityOption => GetQuote<TDOptionQuote>(symbol),
                TDAssetType.Future => GetQuote<TDFutureQuote>(symbol),
                TDAssetType.Forex => GetQuote<TDForexQuote>(symbol),
                _ => throw new ArgumentException($"Error getting quote: asset type {assetType} is currently unsupported")
            };
        }

        private async Task<TDQuote> GetQuote<T>(string symbol) where T : TDQuote
        {
            //var key = HttpUtility.UrlEncode(_authenticator.AppConsumerKey);
            var key = httpRequestMessageService.ApiKey;

            //var result = await http.Client.TryRequestAsync(_authenticator.Token.AccessToken!, $"https://api.TDAmeritrade.com/v1/marketdata/{symbol}/quotes?apikey={key}", null, null, HttpRequestMethod.Get);
            var result = await SendServiceCall<string>(HttpMethod.Get, $"/marketdata/{symbol}/quotes?apikey={key}");
            //string json = result.StatusCode == HttpStatusCode.OK ?
            //  await result.Content.ReadAsStringAsync() :
            //  throw (new Exception($"{result.StatusCode} {result.ReasonPhrase}"));
            string json = result;
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                JsonElement inner = document.RootElement
                    .EnumerateObject().First().Value;

                var jsonResult = inner.Deserialize<T>();
                return jsonResult is not null ?
                    jsonResult as TDQuote :
                    throw new JsonException($"Returned asset quote was interpreted as null from json string {json}.");
            }
        }
    }
}
