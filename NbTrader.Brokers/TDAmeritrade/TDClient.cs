using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using LanguageExt.SomeHelp;
using NbTrader.Brokers.TDAmeritrade.Models;
using NbTrader.Utility;
using System.Net;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Text.Json;
using System.Web;
using NbTrader.Brokers.Extensions;
using static LanguageExt.Prelude;

namespace NbTrader.Brokers.TDAmeritrade
{
    public class TDClient
    {
        private EitherAsync<Error, TDToken> _token;

        private readonly HttpClient _http;
        private readonly string _appConsumerKey;
        private readonly TDAuthenticationService _authService;

        public TDClient(string appConsumerKey)
        {
            _http = new HttpClient();
            _appConsumerKey = appConsumerKey;
            _authService = new TDAuthenticationService(appConsumerKey);
            _authService.TokenUpdated.Subscribe((token) => _token = token);
        }

        #region Quotes
        public EitherAsync<Error, TDQuote> GetQuote(string symbol, TDAssetType assetType)
        {
            return assetType switch
            {
                TDAssetType.Equity => GetQuote<TDEquityQuote>(symbol),
                TDAssetType.EquityOption => GetQuote<TDOptionQuote>(symbol),
                TDAssetType.Future => GetQuote<TDFutureQuote>(symbol),
                TDAssetType.Forex => GetQuote<TDForexQuote>(symbol),
                _ => EitherAsync<Error, TDQuote>.Left(new ArgumentException($"Error getting quote: asset type {assetType} is currently unsupported")),
            };
        }

        private EitherAsync<Error, TDQuote> GetQuote<T>(string symbol) where T : TDQuote
        {
            return GetQuoteJson(symbol).Map<TDQuote>((json) =>
            {
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    JsonElement inner = document.RootElement
                        .EnumerateObject().First().Value;

                    var result = JsonSerializer.Deserialize<T>(inner);
                    return result is null
                        ? throw new JsonException($"Returned asset quote was interpreted as null from json string {json}.")
                        : result as TDQuote;
                }
            });
        }

        /// <summary>
        /// Get quote for a symbol
        /// https://developer.TDAmeritrade.com/quotes/apis/get/marketdata/%7Bsymbol%7D/quotes
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private EitherAsync<Error, string> GetQuoteJson(string symbol)
        {
            EitherAsync<Error, string> result = _token.MapAsync(async token =>
            {
                var key = HttpUtility.UrlEncode(_authService.AppConsumerKey);
                string path = $"https://api.TDAmeritrade.com/v1/marketdata/{symbol}/quotes?apikey={key}";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    var res = await client.GetAsync(path);

                    if (res.StatusCode == HttpStatusCode.OK)
                        return await res.Content.ReadAsStringAsync();
                    else
                        throw (new WebException($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            });
            return result;
        }

        /// <summary>
        /// Get quote for a symbol
        /// https://developer.TDAmeritrade.com/quotes/apis/get/marketdata/%7Bsymbol%7D/quotes
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private EitherAsync<Error, string> GetQuotesJson(string symbol)
        {
            EitherAsync<Error, string> result = _token.MapAsync(async token =>
            {
                var key = HttpUtility.UrlEncode(_authService.AppConsumerKey);

                string path = $"https://api.TDAmeritrade.com/v1/marketdata/quotes";
                var queryParameters = new Dictionary<string, string>()
                {
                    { "apiKey", _authService.AppConsumerKey },
                    { "symbol", symbol.ToUpper().Trim() },
                };
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                    var res = await client.TryRequestAsync(token.AccessToken!, path, queryParameters, null, HttpRequestMethod.Get);

                    if (res.StatusCode == HttpStatusCode.OK)
                        return await res.Content.ReadAsStringAsync();
                    else
                        throw new Exception($"{res.StatusCode} {res.ReasonPhrase}");
                }
            });
            return result;
        }

        #endregion Quotes

        #region AccountInfo
        public EitherAsync<Error, TDAccountInfo[]> GetAccountInfo()
        {
            return _token
                .Bind<string>((token) =>
                {
                    Dictionary<string, string> queryParameters = new() { { "fields", "positions,orders" } };
                    return GetJson(token.AccessToken!, $"https://api.TDAmeritrade.com/v1/accounts", queryParameters, null, HttpRequestMethod.Get);
                })
                .Map<TDAccountInfo[]>((json) =>
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    using (JsonDocument document = JsonDocument.Parse(json))
                    {
                        IList<TDAccountInfo> tradingAccounts = new List<TDAccountInfo>();
                        foreach (var element in document.RootElement.EnumerateArray())
                        {
                            var elementProperty = element.GetProperty("securitiesAccount");
                            var elementPropertyString = elementProperty.ToString();
                            var result = JsonSerializer.Deserialize<TDAccountInfo>(elementPropertyString, options)
                                ?? throw new JsonException($"Returned asset quote was interpreted as null from json string {json}.");

                            tradingAccounts.Add(result);
                        }
                        return tradingAccounts.ToArray();
                    }
                });
        }
        #endregion AccountInfo

        #region Historical Prices

        public EitherAsync<Error, Seq<TDPriceCandle>> GetPrices(TDPriceHistoryRequest request)
        {
            return GetPricesJson(request).Map(json =>
            {
                using (var doc = JsonDocument.Parse(json))
                {
                    JsonElement candles = doc.RootElement.GetProperty("candles");
                    var result = JsonSerializer.Deserialize<TDPriceCandle[]>(candles).ToSeq();
                    return result;
                }
            });
        }

        public EitherAsync<Error, string> GetPricesJson(TDPriceHistoryRequest request)
        {
            return _token.MapAsync<string>(async (token) =>
            {
                var key = HttpUtility.UrlEncode(_appConsumerKey);
                var builder = new UriBuilder($"https://api.TDAmeritrade.com/v1/marketdata/{request.symbol}/pricehistory");
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["apikey"] = key;
                if (request.frequencyType.HasValue)
                {
                    query["frequencyType"] = request.frequencyType.ToString();
                    query["frequency"] = request.frequency.ToString();
                }
                if (request.startDate.HasValue)
                {
                    if (request.endDate == null) request.EndDate = DateTime.Now;
                    query["endDate"] = request.endDate!.Value.ToString();
                    query["startDate"] = request.startDate.Value.ToString();
                }
                if (request.periodType.HasValue)
                {
                    query["periodType"] = request.periodType.ToString();
                    query["period"] = request.period.ToString();
                }
                if (request.needExtendedHoursData.HasValue)
                {
                    query["needExtendedHoursData"] = request.needExtendedHoursData.ToString();
                }
                builder.Query = query.ToString();
                string url = builder.ToString();
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                var res = await _http.GetAsync(url);
                if (res.StatusCode == HttpStatusCode.OK)
                    return await res.Content.ReadAsStringAsync();
                else
                    throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
            });
        }
        #endregion Historical Prices

        #region Orders
        //public EitherAsync<Error, Seq<TDOrder>> GetOrders(long accountId, DateTime from, DateTime to, int? maxResults = null, TDOrderStatusType status = TDOrderStatusType.NotDefined)
        //{

        //}
        //public EitherAsync<Error, TDOrder> CreateOrder(TDOrder order)
        //{

        //}

        #endregion Orders
        private EitherAsync<Error, string> GetJson(string token, string url, IEnumerable<KeyValuePair<string, string>>? queryParams, IEnumerable<KeyValuePair<string, string>>? bodyParams, HttpRequestMethod method = HttpRequestMethod.Get)
        {
            return TryAsync(async () =>
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (queryParams is null)
                    throw new ArgumentNullException(nameof(queryParams));
                if (bodyParams is null)
                    throw new ArgumentNullException(nameof(bodyParams));

                var res = await _http.TryRequestAsync(token, url, queryParams, bodyParams, method);

                if (res.StatusCode == HttpStatusCode.OK)
                    return await res.Content.ReadAsStringAsync();
                else
                    throw Error.New($"HttpRequest error: code=>[{res.StatusCode}], reason=>[{res.ReasonPhrase}]");
            }).ToEither();
        }
    }
}