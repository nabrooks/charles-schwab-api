using LanguageExt;
using LanguageExt.Common;
using NbTrader.Brokers.TDAmeritrade.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Web;
using NbTrader.Brokers.Extensions;
using NbTrader.Brokers.TDAmeritrade.Utilities;
using NbTrader.Utility.Serialization;
using static LanguageExt.Prelude;

namespace NbTrader.Brokers.TDAmeritrade
{
    public class TDAmeritradeClient
    {
        #region Private Static Fields

        private static string BaseAuthenticationUrl = "https://auth.TDAmeritrade.com";
        private static string AuthenticationEndpoint = BaseAuthenticationUrl + "/auth?";

        #endregion

        #region Private Fields

        private string _appConsumerKey;
        private ITdPersistentCache _cache;
        private JsonSerializerOptions _caseNoMatterOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private TDAuthResult? AuthResult;

        #endregion Private Fields

        private TDAmeritradeClient(ITdPersistentCache cache, string appConsumerKey)
        {
            _cache = cache;
            _appConsumerKey = appConsumerKey;
            var redirectServer = TDRedirectServer.Instance;
            redirectServer.AddHandler(async (ctx) =>
            {
                var path = "https://api.TDAmeritrade.com/v1/oauth2/token";
                var queryString = ctx.Request.Query.Querystring;
                var authorizationCodeDecrypted = WebUtility.UrlDecode(queryString.Remove(0, 5));
                using (var client = new HttpClient())
                {
                    var dict = new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_code" },
                        { "access_type", "offline" },
                        { "code", authorizationCodeDecrypted },
                        { "client_id", appConsumerKey + "@AMER.OAUTHAP" },
                        { "redirect_uri", TDRedirectServer.Instance.RedirectUrl }
                    };
                    var req = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, path) { Content = new FormUrlEncodedContent(dict) };
                    var res = await client.SendAsync(req);
                    var now = DateTime.Now;
                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        throw (new AuthenticationException($"{res.StatusCode} {res.ReasonPhrase}"));

                    }
                    var json = await res.Content.ReadAsStringAsync();

                    AuthResult = AuthFrom(json);

                    _cache.Save("TDAmeritradeKey", JsonSerializer.Serialize<TDAuthResult>(AuthResult));
                    HasConsumerKey = true;
                    OnSignedIn(true);
                }
            });

            var authKey = _cache.Load("TDAmeritradeKey");
            if (IsNullOrEmpty(authKey)) return;

            AuthResult = JsonSerializer.Deserialize<TDAuthResult>(authKey);
            if (AuthResult == null) return;

            if (AuthResult.CanRefresh())
                RefreshToken().Wait();
            else
                AuthResult = null;
        }

        public TDAmeritradeClient(string appConsumerKey) : this(new TDUnprotectedCache(), appConsumerKey) { }

        /// <summary>
        /// Client has valid token
        /// </summary>
        public bool IsSignedIn => AuthResult != null && AuthResult.IsValid() && !AuthResult.NeedsRefresh();

        /// <summary>
        /// Client has a consumer key (limited non-authenticated access)
        /// </summary>
        public bool HasConsumerKey { get; private set; }

        /// <summary>
        /// Raised on sign in / out
        /// </summary>
        public event Action<bool> OnSignedIn = delegate { };

        #region Access Token

        /// <summary>
        /// Attempts to sign into td ameritrade services
        /// </summary>
        /// <returns>An (awaitable) <see cref="Task"/></returns>
        public async Task SignIn()
        {
            try
            {
                if (AuthResult == null || AuthResult.IsValid() == false)
                    await GetNewToken();
                else if (AuthResult.NeedsRefresh() && AuthResult.CanRefresh())
                    await RefreshToken();
            }
            catch (Exception) { throw; }
        }

        private async Task GetNewToken()
        {
            var authorizationUrl = $@"{AuthenticationEndpoint}response_type=code&redirect_uri={TDRedirectServer.Instance.RedirectUrl}&client_id={this._appConsumerKey}@AMER.OAUTHAP";

            Process? process;

            try
            {
                process = Process.Start(authorizationUrl);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    process = Process.Start(new ProcessStartInfo("cmd", $"/c start {authorizationUrl.Replace("&", "^&")}") { CreateNoWindow = true });
                    if (process == null)
                        throw new Exception("Browser opening process cannot be null.");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    process = Process.Start("xdg-open", authorizationUrl);
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    process = Process.Start("open", authorizationUrl);
                else
                    throw;
            }

            while (AuthResult == null || AuthResult.IsValid() == false)
                await Task.Delay(100);

            process.Kill();
            process.Close();
            process.Dispose();
            return;
        }

        private async Task RefreshToken()
        {
            if (AuthResult == null || String.IsNullOrWhiteSpace(AuthResult.RefreshToken))
                throw new NullReferenceException("Authentication token object and refresh token string must not be null in order to refresh");

            //var decoded = HttpUtility.UrlDecode(code);
            var path = "https://api.TDAmeritrade.com/v1/oauth2/token";

            var dict = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", AuthResult.RefreshToken },
                { "client_id", _appConsumerKey + "@AMER.OAUTHAP" }
            };

            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, path) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);
                var now = DateTime.Now;

                if (res.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"{res.StatusCode} {res.ReasonPhrase}");

                var json = await res.Content.ReadAsStringAsync();

                AuthResult = AuthFrom(json);

                _cache.Save("TDAmeritradeKey", JsonSerializer.Serialize<TDAuthResult>(AuthResult));
                HasConsumerKey = true;
                OnSignedIn(true);
            }
        }

        #endregion

        #region UserInfo

        /// User Principal details.        
        /// </summary>
        /// <param name="fields">A comma separated String which allows one to specify additional fields to return. None of these fields are returned by default.</param>
        /// <returns></returns>
        public async Task<TDPrincipal> GetPrincipals(params TDPrincipalsFields[] fields)
        {
            var json = await GetPrincipalsJson(fields);

            try
            {
                var result = JsonSerializer.Deserialize<TDPrincipal>(json)
                    ?? throw new NullReferenceException("Returned user principal object was null.");
                return result;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// User Principal details.        
        /// </summary>
        /// <param name="fields">A comma separated String which allows one to specify additional fields to return. None of these fields are returned by default.</param>
        /// <returns></returns>
        private async Task<string> GetPrincipalsJson(params TDPrincipalsFields[] fields)
        {
            if (!IsSignedIn)
            {
                throw (new Exception("Not authenticated"));
            }

            var arg = string.Join(",", fields.Select(o => o.ToString()));

            var path = $"https://api.TDAmeritrade.com/v1/userprincipals?fields={arg}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.GetAsync(path);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion

        #region AccountInfo

        public async Task<IEnumerable<TDAccountInfo>> GetAccounts()
        {
            var json = await GetAccountsJson();
            try
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
                    return tradingAccounts;
                }
            }
            catch (Exception) { throw; }
        }

        private async Task<string> GetAccountsJson()
        {
            var queryParameters = new Dictionary<string, string>()
            {
                { "fields", "positions,orders" }
            };

            if (!IsSignedIn)
                await SignIn();

            string path = $"https://api.TDAmeritrade.com/v1/accounts";

            using (var client = new HttpClient())
            {
                if (IsSignedIn)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);

                var res = await client.TryRequestAsync(AuthResult?.AccessToken!, path, queryParameters, null, HttpRequestMethod.Get);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion AccountInfo

        #region Historical Price

        /// <summary>
        /// Get price history for a symbol
        /// https://developer.TDAmeritrade.com/price-history/apis/get/marketdata/%7Bsymbol%7D/pricehistory
        /// https://developer.TDAmeritrade.com/content/price-history-samples
        /// </summary>
        public async Task<TDPriceCandle[]?> GetPriceHistory(TDPriceHistoryRequest model)
        {
            var json = await GetPriceHistoryJson(model);
            if (!IsNullOrEmpty(json))
            {
                using (var document = JsonDocument.Parse(json))
                {
                    JsonElement candles = document.RootElement.GetProperty("candles");
                    var result = JsonSerializer.Deserialize<TDPriceCandle[]>(candles);
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Get price history for a symbol
        /// https://developer.TDAmeritrade.com/price-history/apis/get/marketdata/%7Bsymbol%7D/pricehistory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> GetPriceHistoryJson(TDPriceHistoryRequest model)
        {
            if (!HasConsumerKey)
            {
                throw (new Exception("ConsumerKey is null"));
            }

            if (!IsSignedIn)
                await SignIn();

            var key = HttpUtility.UrlEncode(AuthResult?.ConsumerKey);

            var builder = new UriBuilder($"https://api.TDAmeritrade.com/v1/marketdata/{model.symbol}/pricehistory");
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (!IsSignedIn)
            {
                query["apikey"] = key;
            }
            if (model.frequencyType.HasValue)
            {
                query["frequencyType"] = model.frequencyType.ToString();
                query["frequency"] = model.frequency.ToString();
            }
            if (model.startDate.HasValue)
            {
                if (model.endDate == null) model.EndDate = DateTime.Now;
                query["endDate"] = model.endDate!.Value.ToString();
                query["startDate"] = model.startDate.Value.ToString();
            }
            if (model.periodType.HasValue)
            {
                query["periodType"] = model.periodType.ToString();
                query["period"] = model.period.ToString();
            }
            if (model.needExtendedHoursData.HasValue)
            {
                query["needExtendedHoursData"] = model.needExtendedHoursData.ToString();
            }
            builder.Query = query.ToString();
            string url = builder.ToString();

            using (var client = new HttpClient())
            {
                if (IsSignedIn)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.GetAsync(url);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion Historical Price

        #region Options

        /// <summary>
        /// Get option chain for an optionable Symbol
        /// https://developer.TDAmeritrade.com/option-chains/apis/get/marketdata/chains
        /// </summary>
        public async Task<TDOptionChain> GetOptionsChain(TDOptionChainRequest request)
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new JsonFloatConverter());
            jsonOptions.Converters.Add(new JsonDoubleConverter());

            var json = await GetOptionsChainJson(request);
            try
            {
                var result = JsonSerializer.Deserialize<TDOptionChain>(json, jsonOptions)
                    ?? throw new JsonException($"Returned option chain was interpreted as null from json string {json}.");

                return result;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Get option chain for an optionable Symbol
        /// https://developer.TDAmeritrade.com/option-chains/apis/get/marketdata/chains
        /// </summary>
        public async Task<string> GetOptionsChainJson(TDOptionChainRequest request)
        {
            if (!HasConsumerKey)
                throw (new Exception("ConsumerKey is null"));

            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            if (!IsSignedIn)
            {
                queryString.Add("apikey", AuthResult?.ConsumerKey);
            }
            queryString.Add("symbol", request.symbol);
            if (request.contractType.HasValue)
            {
                queryString.Add("contractType", request.contractType.ToString());
            }
            if (request.strikeCount.HasValue)
            {
                queryString.Add("strikeCount", request.strikeCount.ToString());
            }
            queryString.Add("includeQuotes", request.includeQuotes ? "FALSE" : "TRUE");
            if (request.interval.HasValue)
            {
                queryString.Add("interval", request.interval.ToString());
            }
            if (request.strike.HasValue)
            {
                queryString.Add("strike", request.strike.Value.ToString());
            }
            if (request.fromDate.HasValue)
            {
                queryString.Add("fromDate", request.fromDate.Value.ToString("yyyy-MM-dd"));
            }
            if (request.toDate.HasValue)
            {
                queryString.Add("toDate", request.toDate.Value.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(request.expMonth))
            {
                queryString.Add("expMonth", request.expMonth);
            }
            queryString.Add("optionType", request.optionType.ToString());

            if (request.strategy == TDOptionChainStrategy.ANALYTICAL)
            {
                queryString.Add("volatility", request.volatility.ToString());
                queryString.Add("underlyingPrice", request.underlyingPrice.ToString());
                queryString.Add("interestRate", request.interestRate.ToString());
                queryString.Add("daysToExpiration", request.daysToExpiration.ToString());
            }

            var q = queryString.ToString();

            var path = $"https://api.TDAmeritrade.com/v1/marketdata/chains?{q}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.GetAsync(path);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion

        #region Instruments

        public async Task<IEnumerable<TDInstrumentModel>> GetInstruments(string[] instrumentSymbols)
        {
            String? instrumentSymbolsJoined = String.Join(",", instrumentSymbols) ?? throw new ArgumentException("Error joining symbols");
            try
            {
                var json = await GetInstrumentJson(instrumentSymbolsJoined);

                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    List<TDInstrumentModel> instrumentList = new List<TDInstrumentModel>();
                    foreach (var obj in document.RootElement.EnumerateObject())
                    {
                        var result = JsonSerializer.Deserialize<TDInstrumentModel>(obj.Value, _caseNoMatterOptions)
                            ?? throw new JsonException($"Returned instruments were unable to be interpreted from json string {json}.");
                        instrumentList.Add(result);
                    }
                    return instrumentList;
                }
            }
            catch (Exception) { throw; }
        }

        public async Task<TDInstrumentModel> GetInstrument(string instrumentSymbol)
        {
            var json = await GetInstrumentJson(instrumentSymbol);
            try
            {
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    var instrumentJson = document.RootElement.GetProperty(instrumentSymbol);
                    var result = JsonSerializer.Deserialize<TDInstrumentModel>(instrumentJson, _caseNoMatterOptions)
                        ?? throw new JsonException($"Returned asset quote was interpreted as null from json string {json}.");
                    return result;
                }
            }
            catch (Exception) { throw; }
        }

        private async Task<string> GetInstrumentJson(string instrumentSymbol)
        {
            if (!IsSignedIn)
                await SignIn();
            var key = HttpUtility.UrlEncode(AuthResult?.ConsumerKey);
            var uri = $"https://api.TDAmeritrade.com/v1/instruments";
            var symbolUpper = instrumentSymbol?.ToUpper()?.Trim();

            var queryParameters = new Dictionary<string, string>()
            {
                { "apiKey", _appConsumerKey },
                { "symbol", symbolUpper ?? "" },
                { "projection", "symbol-search" }
            };
            using (var client = new HttpClient())
            {

                if (IsSignedIn)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.TryRequestAsync(AuthResult?.AccessToken, uri, queryParameters, null, HttpRequestMethod.Get);
                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion

        #region Quotes

        public EitherAsync<Error, TDQuote> GetQuote(string symbol, TDAssetType assetType)
        {
            return assetType switch
            {
                TDAssetType.Equity => GetEquityQuote(symbol),
                TDAssetType.EquityOption => GetOptionQuote(symbol),
                TDAssetType.Future => GetFutureQuote(symbol),
                TDAssetType.Forex => GetForexQuote(symbol),
                _ => EitherAsync<Error, TDQuote>.Left(new ArgumentException($"Error getting quote: asset type {assetType} is currently unsupported")),
            };
        }

        private EitherAsync<Error, TDQuote> GetEquityQuote(string symbol) => GetQuoteE<TDEquityQuote>(symbol);
        private EitherAsync<Error, TDQuote> GetFutureQuote(string symbol) => GetQuoteE<TDFutureQuote>(symbol);
        private EitherAsync<Error, TDQuote> GetOptionQuote(string symbol) => GetQuoteE<TDOptionQuote>(symbol);
        private EitherAsync<Error, TDQuote> GetForexQuote(string symbol) => GetQuoteE<TDForexQuote>(symbol);
        private EitherAsync<Error, TDQuote> GetEquityQuoteE(string symbol) => GetQuoteE<TDEquityQuote>(symbol);

        private EitherAsync<Error, TDQuote> GetQuoteE<T>(string symbol) where T : TDQuote =>
            TryAsync(async () =>
            {
                var json = await GetQuoteJson(symbol);
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    JsonElement inner = document.RootElement
                        .EnumerateObject().First().Value;

                    var result = JsonSerializer.Deserialize<T>(inner);
                    if (result is null)
                        throw new JsonException($"Returned asset quote was interpreted as null from json string {json}.");
                    else
                        return result as TDQuote;
                }
            })
            .ToEither(error => error);

        ///// <summary>
        ///// Get quote for a symbol
        ///// https://developer.TDAmeritrade.com/quotes/apis/get/marketdata/%7Bsymbol%7D/quotes
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="symbol"></param>
        ///// <returns></returns>
        //public async Task<Either<Exception,TDQuote>> GetQuote<T>(string symbol) where T : TDQuote
        //{
        //    var json = await GetQuoteJson(symbol);
        //    try
        //    {
        //        using (JsonDocument document = JsonDocument.Parse(json))
        //        {
        //            JsonElement inner = document.RootElement
        //                .EnumerateObject().First().Value;
                    
        //            var result = JsonSerializer.Deserialize<T>(inner);
        //            if (result is null)
        //                return Either<Exception, TDQuote>.Left(new JsonException($"Returned asset quote was interpreted as null from json string {json}."));
        //            else
        //                return Either<Exception, TDQuote>.Right((T)(result as TDQuote));
        //        }
        //    }
        //    catch (Exception ex) {
        //        return Either<Exception, TDQuote>.Left(ex);
        //    }
        //}

        ///// <summary>
        ///// Get quote for a symbol
        ///// https://developer.TDAmeritrade.com/quotes/apis/get/marketdata/%7Bsymbol%7D/quotes
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="symbol"></param>
        ///// <returns></returns>
        //private async Task<Result<T>> GetQuotes<T>(string symbol) where T : TDQuoteBase
        //{
        //    var json = await GetQuotesJson(symbol);
        //    try
        //    {
        //        using (JsonDocument document = JsonDocument.Parse(json))
        //        {
        //            var inner = document.RootElement
        //               .EnumerateObject().First().Value;

        //            var result = JsonSerializer.Deserialize<T>(inner);
        //            if (result is null)
        //                return new Result<T>(new JsonException($"Returned asset quote was interpreted as null from json string {json}."));
        //            else
        //                return new Result<T>(result);
        //        }
        //    }
        //    catch (Exception) { throw; }
        //}

        /// <summary>
        /// Get quote for a symbol
        /// https://developer.TDAmeritrade.com/quotes/apis/get/marketdata/%7Bsymbol%7D/quotes
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private async Task<string> GetQuoteJson(string symbol)
        {
            if (!HasConsumerKey)
            {
                throw (new Exception("ConsumerKey is null"));
            }

            var key = HttpUtility.UrlEncode(AuthResult?.ConsumerKey);

            string path = IsSignedIn
                ? $"https://api.TDAmeritrade.com/v1/marketdata/{symbol}/quotes"
                : $"https://api.TDAmeritrade.com/v1/marketdata/{symbol}/quotes?apikey={key}";

            using (var client = new HttpClient())
            {
                if (IsSignedIn)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.GetAsync(path);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        /// <summary>
        /// Get quote for a symbol
        /// https://developer.TDAmeritrade.com/quotes/apis/get/marketdata/%7Bsymbol%7D/quotes
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private async Task<string> GetQuotesJson(string symbol)
        {
            if (!HasConsumerKey)
                throw (new Exception("ConsumerKey is null"));

            var key = HttpUtility.UrlEncode(AuthResult?.ConsumerKey);

            string path = $"https://api.TDAmeritrade.com/v1/marketdata/quotes";
            var queryParameters = new Dictionary<string, string>()
            {
                { "apiKey", _appConsumerKey },
                { "symbol", symbol.ToUpper().Trim() },
            };
            using (var client = new HttpClient())
            {
                if (IsSignedIn)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);

                var res = await client.TryRequestAsync(AuthResult?.AccessToken!, path, queryParameters, null, HttpRequestMethod.Get);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion

        #region Orders

        public async Task<IEnumerable<TDOrder>> GetOrders(long accountId, TDOrderStatusType? orderStatusType, DateTime? fromDateTime = null, DateTime? toDateTime = null)
        {
            if (!IsSignedIn)
                await SignIn();

            if (fromDateTime != null && toDateTime != null && toDateTime < fromDateTime)
                throw new ArgumentException("endDate must be after (or greater than) start date");
            if (toDateTime != null && fromDateTime != null && (toDateTime - fromDateTime).Value.TotalDays > 365)
                throw new ArgumentException("endDate cannot be more than a year ahead of start date");

            // Assumptions about datetime interval
            if (toDateTime == null && fromDateTime == null) // Assumes client wants a years worth of Transactions starting Now if interval is null 
                toDateTime = DateTime.Now;
            if (fromDateTime == null) // Assumes client wants a years worth of Transactions starting a year before the end date if start date is null and enddate is not null
                fromDateTime = toDateTime - TimeSpan.FromDays(365);
            if (toDateTime == null && fromDateTime != null) // if enddate is not defined, but start date is, assume a years worth of Transactions to be requested from start date
                toDateTime = fromDateTime + TimeSpan.FromDays(365);
            var orderStatusString = JsonSerializer.Serialize(orderStatusType).Replace("\"", "");


            var queryParams = new Dictionary<string, string>()
            {
                { "accountId", accountId.ToString() },
                { "maxResults", int.MaxValue.ToString() },
            };

            if (fromDateTime != null)
                queryParams.Add("fromEnteredTime", fromDateTime.FormatToString("yyyy-MM-dd"));
            if (toDateTime != null)
                queryParams.Add("toEnteredTime", toDateTime.FormatToString("yyyy-MM-dd"));

            queryParams.Add("status", orderStatusString);
            using (var client = new HttpClient())
            {
                var responseMessage = await client.TryRequestAsync(AuthResult?.AccessToken, $"https://api.TDAmeritrade.com/v1/orders", queryParams, null, HttpRequestMethod.Get);
                var responseMessageContentString = await responseMessage.Content.ReadAsStringAsync();
                using (JsonDocument document = JsonDocument.Parse(responseMessageContentString))
                {
                    if (responseMessage.IsSuccessStatusCode == false)
                        throw new HttpRequestException($"The response from the server indicates an unsuccessful server message: '{document.RootElement.GetProperty("error")}'. Status Code: '{responseMessage.StatusCode}'");

                    var elementProperty = document.RootElement;
                    var elementPropertyString = elementProperty.ToString();

                    var elementObjects = JsonSerializer.Deserialize<IEnumerable<TDOrder>>(elementPropertyString, _caseNoMatterOptions)
                        ?? throw new JsonException("Cannot deserialize to IEnumerable<TDOrderModel>");

                    return elementObjects;
                }
            }
        }


        /// <summary>
        /// Gets a preexisting order
        /// </summary>
        /// <param name="accountId">The id of the account</param>
        /// <param name="orderId">The id of the preexisting order</param>
        /// <returns>An order model object</returns>
        /// <exception cref="JsonException">Occurs when returned json string is un-parsable</exception>
        /// <exception cref="Exception">Occurs when the HttpClient returns with a status code other than 200 (OK)</exception>
        public async Task<TDOrder> GetOrder(long accountId, long orderId)
        {
            if (!IsSignedIn)
                await SignIn();

            var url = $"https://api.TDAmeritrade.com/v1/accounts/{accountId}/orders/{orderId}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.GetAsync(url);

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    var orderModel = JsonSerializer.Deserialize<TDOrder>(json, _caseNoMatterOptions) ?? throw new JsonException("Couldnt parse get order response");
                    return orderModel;
                }
                else throw new Exception($"{res.StatusCode} {res.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Requests an order be created
        /// </summary>
        /// <param name="orderModel">The order information to provide server</param>
        /// <returns>an (awaitable) <see cref="Task"/></returns>
        /// <exception cref="Exception">If HttpStatusCode is not "OK"</exception>
        public async Task CreateOrder(TDOrder orderModel)
        {
            if (!IsSignedIn)
                await SignIn();

            var url = $"https://api.TDAmeritrade.com/v1/accounts/{orderModel.AccountId}/orders";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                string json = JsonSerializer.Serialize(orderModel, _caseNoMatterOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PostAsync(url, content);

                if (res.StatusCode == HttpStatusCode.OK) return;
                else throw new Exception($"{res.StatusCode} {res.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Requests an order be updated
        /// </summary>
        /// <param name="orderModel">The order information to provide server</param>
        /// <returns>an (awaitable) <see cref="Task"/></returns>
        /// <exception cref="Exception">If HttpStatusCode is not "OK"</exception>
        public async Task UpdateOrder(TDOrder orderModel)
        {
            if (!IsSignedIn)
                await SignIn();

            var url = $"https://api.TDAmeritrade.com/v1/accounts/{orderModel.AccountId}/orders/{orderModel.OrderId}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                string json = JsonSerializer.Serialize(orderModel, _caseNoMatterOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = await client.PostAsync(url, content);

                if (res.StatusCode == HttpStatusCode.OK) return;
                else throw new Exception($"{res.StatusCode} {res.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Requests an order be deleted
        /// </summary>
        /// <param name="accountId">The account to delete the order in</param>
        /// <param name="orderId">The unique id of the order to delete</param>
        /// <returns>an (awaitable) <see cref="Task"/></returns>
        /// <exception cref="Exception">If HttpStatusCode is not "OK"</exception>
        public async Task CancelOrder(long accountId, long orderId)
        {
            if (!IsSignedIn)
                await SignIn();

            var url = $"https://api.TDAmeritrade.com/v1/accounts/{accountId}/orders/{orderId}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                var res = await client.DeleteAsync(url);//.PostAsync(url, content);

                if (res.StatusCode == HttpStatusCode.OK) return;
                else throw new Exception($"{res.StatusCode} {res.ReasonPhrase}");
            }
        }

        #endregion Orders

        #region Market Hours

        /// <summary>
        /// Retrieve market hours for specified single market
        /// </summary>
        public async Task<TDMarketHour> GetMarketHours(MarketTypes type, DateTime day)
        {
            var json = await GetMarketHoursJson(type, day);
            try
            {
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    JsonElement market = document.RootElement
                        .EnumerateObject().First().Value
                        .EnumerateObject().First().Value;

                    TDMarketHour? obj = JsonSerializer.Deserialize<TDMarketHour>(market.ToString())
                        ?? throw new JsonException($"Returned market deserialization result is null from json string {market.ToString()}.");

                    return obj;
                }
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Retrieve market hours for specified single market
        /// </summary>
        private async Task<string> GetMarketHoursJson(MarketTypes type, DateTime day)
        {
            if (!IsSignedIn)
                await SignIn();

            var key = HttpUtility.UrlEncode(AuthResult?.ConsumerKey);
            var dayString = day.ToString("yyyy-MM-dd").Replace("/", "-");
            string path = IsSignedIn
                ? $"https://api.TDAmeritrade.com/v1/marketdata/{type}/hours?date={dayString}"
                : $"https://api.TDAmeritrade.com/v1/marketdata/{type}/hours?apikey={key}&date={dayString}";

            using (var client = new HttpClient())
            {
                if (IsSignedIn)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult?.AccessToken);
                }
                var res = await client.GetAsync(path);

                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await res.Content.ReadAsStringAsync();
                    default:
                        throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
            }
        }

        #endregion

        #region Utility  

        private static TDAuthResult AuthFrom(string json)
        {
            using (JsonDocument jsonDoc =  JsonDocument.Parse(json))
            {
                var now = DateTime.Now;
                var jRoot = jsonDoc.RootElement;
                var authResult = new TDAuthResult()
                {
                    RedirectUrl = jRoot.GetProperty("redirect_url").GetString(),
                    ConsumerKey = jRoot.GetProperty("consumer_key").GetString(),
                    SecurityCode = jRoot.GetProperty("security_code").GetString(),
                    AccessToken = jRoot.GetProperty("access_token").GetString(),
                    RefreshToken = jRoot.GetProperty("access_token").GetString(),
                    Scope = jRoot.GetProperty("access_token").GetString(),
                    Expiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("expires_in").GetDouble()),
                    RefreshTokenExpiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("refresh_token_expires_in").GetDouble()),
                    TokenType = jRoot.GetProperty("token_type").GetString(),
                };
                return authResult;
            }
        }

        private static bool IsNullOrEmpty(string s)
        {
            return string.IsNullOrEmpty(s) || s == "{}";
        }

        #endregion
    }
}