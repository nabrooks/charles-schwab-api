using System.Text.Json;
using System.Text.Json.Serialization;
using NbTrader.Brokers.Extensions;
using NbTrader.Brokers.TDAmeritrade.Utilities;

namespace NbTrader.Brokers.TDAmeritrade.Models
{
    [Serializable]
    public enum TDOptionChainTypes
    {
        ALL,
        PUT,
        CALL
    }

    [Serializable]
    public enum TDOptionChainStrategy
    {
        SINGLE,
        ANALYTICAL,
        COVERED,
        VERTICAL,
        CALENDAR,
        STRANGLE,
        STRADDLE,
        BUTTERFLY,
        CONDOR,
        DIAGNOL,
        COLLAR,
        ROLL
    }

    [Serializable]
    public enum TDOptionChainOptionTypes
    {
        /// <summary>
        /// All
        /// </summary>
        ALL,
        /// <summary>
        /// Standard
        /// </summary>
        S,
        /// <summary>
        /// NonStandard
        /// </summary>
        NS
    }

    /// <summary>
    /// ITM: In-the-money
    /// NTM: Near-the-money
    /// OTM: Out-of-the-money
    /// SAK: Strikes Above Market
    /// SBK: Strikes Below Market
    /// SNK: Strikes Near Market
    /// ALL: All Strikes
    /// </summary>
    [Serializable]
    public enum TDOptionChainRanges
    {
        /// ALL: All Strikes
        ALL,
        /// ITM: In-the-money
        ITM,
        /// NTM: Near-the-money
        NTM,
        /// OTM: Out-of-the-money
        OTM,
        /// SAK: Strikes Above Market
        SAK,
        /// SBK: Strikes Below Market
        SBK,
        /// SNK: Strikes Near Market
        SNK,
    }

    [Serializable]
    public class TDOptionChainRequest
    {
        /// <summary>
        /// security id
        /// </summary>
        public string? symbol { get; set; }
        /// <summary>
        /// The number of strikes to return above and below the at-the-money price.
        /// </summary>
        public int? strikeCount { get; set; }
        /// <summary>
        /// Passing a value returns a Strategy Chain
        /// </summary>
        public TDOptionChainStrategy strategy { get; set; }
        /// <summary>
        /// Type of contracts to return in the chai
        /// </summary>
        public TDOptionChainTypes? contractType { get; set; }
        /// <summary>
        /// Only return expirations after this date
        /// </summary>
        public DateTime? fromDate { get; set; }
        /// <summary>
        /// Only return expirations before this date
        /// </summary>
        public DateTime? toDate { get; set; }

        /// <summary>
        /// Strike interval for spread strategy chains
        /// </summary>
        public double? interval {get;set; }
        /// <summary>
        /// Provide a strike price to return options only at that strike price.
        /// </summary>
        public double? strike { get; set; }
        /// <summary>
        /// Returns options for the given range
        /// </summary>
        public TDOptionChainRanges range { get; set; }
        /// <summary>
        /// Volatility to use in calculations. ANALYTICAL  only.
        /// </summary>
        public double? volatility { get; set; }
        /// <summary>
        /// Underlying price to use in calculations. ANALYTICAL  only.
        /// </summary>
        public double underlyingPrice { get; set; }
        /// <summary>
        /// Interest rate to use in calculations. ANALYTICAL  only.
        /// </summary>
        public double interestRate { get; set; }
        /// <summary>
        /// Days to expiration to use in calculations. Applies only to ANALYTICAL strategy chains
        /// </summary>
        public int? daysToExpiration { get; set; }
        /// <summary>
        /// Return only options expiring in the specified month
        /// </summary>
        public string? expMonth { get; set; }
        /// <summary>
        /// Include quotes for options in the option chain. Can be TRUE or FALSE. Default is FALSE.
        /// </summary>
        public bool includeQuotes { get; set; }
        /// <summary>
        /// Type of contracts to return
        /// </summary>
        public TDOptionChainOptionTypes optionType { get; set; }
    }

    [Serializable]
    [JsonConverter(typeof(TDOptionChainConverter))]
    public class TDOptionChain
    {
        public string? symbol { get; set; }
        public string? status { get; set; }
        public TDUnderlying? underlying { get; set; }
        public string? strategy { get; set; }
        public double interval { get; set; }
        public bool isDelayed { get; set; }
        public bool isIndex { get; set; }
        public double daysToExpiration { get; set; }
        public double interestRate { get; set; }
        public double underlyingPrice { get; set; }
        public double volatility { get; set; }

        public List<TDOptionMap>? callExpDateMap { get; set; }
        public List<TDOptionMap>? putExpDateMap { get; set; }
    }

    public class TDOptionChainConverter : JsonConverter<TDOptionChain>
    {
        public override TDOptionChain? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
            }

            // try to parse number directly from bytes
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var doc = document.RootElement;
                var model = new TDOptionChain();
                model.symbol = doc.GetProperty("symbol").Value<string>(options);
                model.status = doc.GetProperty("status").Value<string>(options);
                model.underlying = doc.GetProperty("underlying").Value<TDUnderlying>(options);
                model.strategy = doc.GetProperty("strategy").Value<string>(options);
                model.interval = doc.GetProperty("interval").Value<float>(options);
                model.isDelayed = doc.GetProperty("isDelayed").Value<bool>(options);
                model.isIndex = doc.GetProperty("isIndex").Value<bool>(options);
                model.daysToExpiration = doc.GetProperty("daysToExpiration").Value<double>(options);
                model.interestRate = doc.GetProperty("interestRate").Value<double>(options);
                model.underlyingPrice = doc.GetProperty("underlyingPrice").Value<double>(options);
                model.volatility = doc.GetProperty("volatility").Value<double>(options);
                model.callExpDateMap = GetMap(doc.GetProperty("callExpDateMap"), options);
                model.putExpDateMap = GetMap(doc.GetProperty("putExpDateMap"), options);
                return model;
            }



            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TDOptionChain value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public List<TDOptionMap> GetMap(JsonElement doc, JsonSerializerOptions? options = null)
        {
            var map = new List<TDOptionMap>();

            foreach (JsonProperty expiry in doc.EnumerateObject())
            {
                var exp = new TDOptionMap();
                map.Add(exp);
                exp.expires = DateTime.Parse(expiry.Name.Split(':')[0]);
                exp.options = new List<TDOption>();

                var set = expiry.Value;//.ToObject<JObject>();
                foreach (var contract in set.EnumerateObject())
                {
                    var strike = double.Parse(contract.Name);
                    var tuples = contract.Value.EnumerateArray().First();//.ToObject<JObject>();

                    var option = new TDOption();
                    exp.options.Add(option);

                    option.putCall = tuples.GetProperty("putCall").Value<string>(options);
                    option.symbol = tuples.GetProperty("symbol").Value<string>(options);
                    option.description = tuples.GetProperty("description").Value<string>(options);
                    option.exchangeName = tuples.GetProperty("exchangeName").Value<string>(options);
                    option.bidPrice = tuples.GetProperty("bid").Value<double>(options);
                    option.askPrice = tuples.GetProperty("ask").Value<double>(options);
                    option.lastPrice = tuples.GetProperty("last").Value<double>(options);
                    option.markPrice = tuples.GetProperty("mark").Value<double>(options);
                    option.bidSize = tuples.GetProperty("bidSize").Value<int>(options);
                    option.askSize = tuples.GetProperty("askSize").Value<int>(options);
                    option.lastSize = tuples.GetProperty("lastSize").Value<int>(options);
                    option.highPrice = tuples.GetProperty("highPrice").Value<double>(options);
                    option.lowPrice = tuples.GetProperty("lowPrice").Value<double>(options);
                    option.openPrice = tuples.GetProperty("openPrice").Value<double>(options);
                    option.closePrice = tuples.GetProperty("closePrice").Value<double>(options);
                    option.totalVolume = tuples.GetProperty("totalVolume").Value<int>(options);
                    option.quoteTimeInLong = tuples.GetProperty("quoteTimeInLong").Value<long>(options);
                    option.tradeTimeInLong = tuples.GetProperty("tradeTimeInLong").Value<long>(options);
                    option.netChange = tuples.GetProperty("netChange").Value<double>(options);
                    option.volatility = tuples.GetProperty("volatility").Value<double>(options);
                    option.delta = tuples.GetProperty("delta").Value<double>(options);
                    option.gamma = tuples.GetProperty("gamma").Value<double>(options);
                    option.theta = tuples.GetProperty("theta").Value<double>(options);
                    option.vega = tuples.GetProperty("vega").Value<double>(options);
                    option.rho = tuples.GetProperty("rho").Value<double>(options);
                    option.timeValue = tuples.GetProperty("timeValue").Value<double>(options);
                    option.openInterest = tuples.GetProperty("openInterest").Value<int>(options);
                    option.isInTheMoney = tuples.GetProperty("inTheMoney").Value<bool>(options);
                    option.theoreticalOptionValue = tuples.GetProperty("theoreticalOptionValue").Value<double>(options);
                    option.theoreticalVolatility = tuples.GetProperty("theoreticalVolatility").Value<double>(options);
                    option.strikePrice = tuples.GetProperty("strikePrice").Value<double>(options);
                    option.expirationDate = tuples.GetProperty("expirationDate").Value<double>(options);
                    option.daysToExpiration = tuples.GetProperty("daysToExpiration").Value<int>(options);
                    option.multiplier = tuples.GetProperty("multiplier").Value<double>(options);
                    option.settlementType = tuples.GetProperty("settlementType").Value<string>(options);
                    option.deliverableNote = tuples.GetProperty("deliverableNote").Value<string>(options);
                    option.percentChange = tuples.GetProperty("percentChange").Value<double>(options);
                    option.markChange = tuples.GetProperty("markChange").Value<double>(options);
                    option.markPercentChange = tuples.GetProperty("markPercentChange").Value<double>(options);

                }
            }
            return map;
        }

    }
    
    [Serializable]
    public class TDOptionMap
    {
        public DateTime expires { get; set; }

        public List<TDOption>? options { get; set; }
    }



    [Serializable]
    public class TDOption
    {
        public string? putCall { get; set; }
        public string? symbol { get; set; }
        public string? description { get; set; }
        public string? exchangeName { get; set; }
        public double bidPrice { get; set; }
        public double askPrice { get; set; }
        public double lastPrice { get; set; }
        public double markPrice { get; set; }
        public int bidSize { get; set; }
        public int askSize { get; set; }
        public int lastSize { get; set; }
        public double highPrice { get; set; }
        public double lowPrice { get; set; }
        public double openPrice { get; set; }
        public double closePrice { get; set; }
        public int totalVolume { get; set; }
        public long quoteTimeInLong { get; set; }
        public long tradeTimeInLong { get; set; }
        public double netChange { get; set; }
        public double volatility { get; set; }
        public double delta { get; set; }
        public double gamma { get; set; }
        public double theta { get; set; }
        public double vega { get; set; }
        public double rho { get; set; }
        public double timeValue { get; set; }
        public int openInterest { get; set; }
        public bool isInTheMoney { get; set; }
        public double theoreticalOptionValue { get; set; }
        public double theoreticalVolatility { get; set; }
        public double strikePrice { get; set; }
        public double expirationDate { get; set; }
        public int daysToExpiration { get; set; }
        public string? expirationType { get; set; }
        public double multiplier { get; set; }
        public string? settlementType { get; set; }
        public string? deliverableNote { get; set; }
        public double percentChange { get; set; }
        public double markChange { get; set; }
        public double markPercentChange { get; set; }
        public DateTime ExpirationDate
        {
            get
            {
                return TDHelpers.FromUnixTimeMilliseconds(expirationDate);
            }
        }
        public DateTime ExpirationDay
        {
            get
            {
                return TDHelpers.ToRegularTradingEnd(DateTime.Now.AddDays(daysToExpiration));
            }
        }
    }

    [Serializable]
    public class TDUnderlying
    {
        public double ask { get; set; }
        public int askSize { get; set; }
        public double bid { get; set; }
        public int bidSize { get; set; }
        public double change { get; set; }
        public double close { get; set; }
        public bool delayed { get; set; }
        public string? description { get; set; }
        public string? exchangeName { get; set; }
        public double fiftyTwoWeekHigh { get; set; }
        public double fiftyTwoWeekLow { get; set; }
        public double highPrice { get; set; }
        public double last { get; set; }
        public double lowPrice { get; set; }
        public double mark { get; set; }
        public double markChange { get; set; }
        public double markPercentChange { get; set; }
        public double openPrice { get; set; }
        public double percentChange { get; set; }
        public double quoteTime { get; set; }
        public string? symbol { get; set; }
        public int totalVolume { get; set; }
        public double tradeTime { get; set; }
    }


    [Serializable]
    public class TDExpirationDate
    {
        public string? date { get; set; }
    }

    [Serializable]
    public class TDOptionDeliverables
    {
        public string? symbol { get; set; }
        public string? assetType { get; set; }
        public string? deliverableUnits { get; set; }
        public string? currencyType { get; set; }
    }


}
