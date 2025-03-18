using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NbTrader.Brokers.TDAmeritrade.Models
{
    public abstract class TDQuote
    {
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("exchange")]
        public string? Exchange { get; set; }
        [JsonPropertyName("exchangeName")]
        public string? ExchangeName { get; set; }
        [JsonPropertyName("securityStatus")]
        public string? SecurityStatus { get; set; }
        [JsonPropertyName("mark")]
        public double? Mark { get; set; }
        [JsonPropertyName("tick")]
        public double? Tick { get; set; }
        [JsonPropertyName("tickAmount")]
        public double? TickAmount { get; set; }

        public abstract double BidPrice { get; set; }
        public abstract double BidSize { get; set; }

        public abstract double AskPrice { get; set; }
        public abstract double AskSize { get; set; }
        public abstract double LastPrice { get; set; }
        public abstract double LastSize { get; set; }

        public abstract double OpenPrice { get; set; }
        public abstract double HighPrice { get; set; }
        public abstract double LowPrice { get; set; }
        public abstract double ClosePrice { get; set; }
    }

    public class TDForexQuote : TDQuote
    {
        [JsonPropertyName("bidPriceInDouble")]
        public override double BidPrice { get; set; }
        
        [JsonIgnore]
        public override double BidSize { get; set; } = 0;

        [JsonPropertyName("askPriceInDouble")]
        public override double AskPrice { get; set; }

        [JsonIgnore]
        public override double AskSize { get; set; } = 0;

        [JsonPropertyName("lastPriceInDouble")]
        public override double LastPrice { get; set; }

        [JsonIgnore]
        public override double LastSize { get; set; } = 0;

        [JsonPropertyName("openPriceInDouble")]
        public override double OpenPrice { get; set; }

        [JsonPropertyName("highPriceInDouble")]
        public override double HighPrice { get; set; }

        [JsonPropertyName("lowPriceInDouble")]
        public override double LowPrice { get; set; }

        [JsonPropertyName("closePriceInDouble")]
        public override double ClosePrice { get; set; }

        [JsonPropertyName("changeInDouble")]
        public double Change { get; set; }

        [JsonPropertyName("percentChange")]
        public double PercentChange { get; set; }

        [JsonPropertyName("digits")]
        public double Digits { get; set; }

        [JsonPropertyName("product")]
        public string? Product { get; set; }

        [JsonPropertyName("tradingHours")]
        public string? TradingHours { get; set; }

        [JsonPropertyName("isTradable")]
        public bool IsTradable { get; set; }

        [JsonPropertyName("marketMaker")]
        public string? MarketMaker { get; set; }

        [JsonPropertyName("52WkHighInDouble")]
        public double Week52HighInDouble { get; set; }

        [JsonPropertyName("52WkLowInDouble")]
        public double Week52LowInDouble { get; set; }
    }

    public class TDEquityQuote : TDQuote
    {
        [JsonPropertyName("bidPrice")]
        public override double BidPrice { get; set; }

        [JsonPropertyName("bidSize")]
        public override double BidSize { get; set; }

        [JsonPropertyName("askPrice")]
        public override double AskPrice { get; set; }

        [JsonPropertyName("askSize")]
        public override double AskSize { get; set; }

        [JsonPropertyName("lastPrice")]
        public override double LastPrice { get; set; }

        [JsonPropertyName("lastSize")]
        public override double LastSize { get; set; }

        [JsonPropertyName("openPrice")]
        public override double OpenPrice { get; set; }

        [JsonPropertyName("highPrice")]
        public override double HighPrice { get; set; }

        [JsonPropertyName("lowPrice")]
        public override double LowPrice { get; set; }

        [JsonPropertyName("closePrice")]
        public override double ClosePrice { get; set; }

        [JsonPropertyName("totalVolume")]
        public double TotalVolume { get; set; }

        [JsonPropertyName("quoteTimeInLong")]
        public long QuoteTimeInLong { get; set; }

        [JsonPropertyName("tradeTimeInLong")]
        public long TradeTimeInLong { get; set; }

        [JsonPropertyName("marginable")]
        public bool Marginable { get; set; }

        [JsonPropertyName("shortable")]
        public bool Shortable { get; set; }

        [JsonPropertyName("volatility")]
        public double Volatility { get; set; }

        [JsonPropertyName("digits")]
        public double Digits { get; set; }

        [JsonPropertyName("52WkHigh")]
        public double Week52High { get; set; }

        [JsonPropertyName("52WkLow")]
        public double Week52Low { get; set; }

        [JsonPropertyName("peRatio")]
        public double PeRatio { get; set; }

        [JsonPropertyName("divAmount")]
        public double DivAmount { get; set; }

        [JsonPropertyName("divYield")]
        public double DivYield { get; set; }

        [JsonPropertyName("divDate")]
        public string? DivDate { get; set; }

        [JsonPropertyName("regularMarketLastPrice")]
        public double RegularMarketLastPrice { get; set; }

        [JsonPropertyName("regularMarketLastSize")]
        public double RegularMarketLastSize { get; set; }

        [JsonPropertyName("regularMarketNetChange")]
        public double RegularMarketNetChange { get; set; }

        [JsonPropertyName("regularMarketTradeTimeInLong")]
        public long RegularMarketTradeTimeInLong { get; set; }
    }

    public class TDOptionQuote : TDQuote
    {
        [JsonPropertyName("bidPrice")]
        public override double BidPrice { get; set; }

        [JsonPropertyName("bidSize")]
        public override double BidSize { get; set; }

        [JsonPropertyName("askPrice")]
        public override double AskPrice { get; set; }

        [JsonPropertyName("askSize")]
        public override double AskSize { get; set; }

        [JsonPropertyName("lastPrice")]
        public override double LastPrice { get; set; }

        [JsonPropertyName("lastSize")]
        public override double LastSize { get; set; }

        [JsonPropertyName("openPrice")]
        public override double OpenPrice { get; set; }

        [JsonPropertyName("highPrice")]
        public override double HighPrice { get; set; }

        [JsonPropertyName("lowPrice")]
        public override double LowPrice { get; set; }

        [JsonPropertyName("closePrice")]
        public override double ClosePrice { get; set; }

        [JsonPropertyName("totalVolume")]
        public double TotalVolume { get; set; }

        [JsonPropertyName("quoteTimeInLong")]
        public long QuoteTimeInLong { get; set; }

        [JsonPropertyName("tradeTimeInLong")]
        public long TradeTimeInLong { get; set; }

        [JsonPropertyName("volatility")]
        public double Volatility { get; set; }

        [JsonPropertyName("delta")]
        public double Delta { get; set; }

        [JsonPropertyName("gamma")]
        public double Gamma { get; set; }

        [JsonPropertyName("theta")]
        public double Theta { get; set; }

        [JsonPropertyName("vega")]
        public double Vega { get; set; }

        [JsonPropertyName("rho")]
        public double Rho { get; set; }

        [JsonPropertyName("theoreticalOptionValue")]
        public double TheoreticalOptionValue { get; set; }

        [JsonPropertyName("strikePrice")]
        public double StrikePrice { get; set; }

        [JsonPropertyName("uvExpirationType")]
        public string? ExpirationType { get; set; }

        [JsonPropertyName("multiplier")]
        public double Multiplier { get; set; }

        [JsonPropertyName("settlementType")]
        public string? SettlementType { get; set; }
    }

    public class TDFutureQuote : TDQuote
    {
        [JsonPropertyName("bidPriceInDouble")]
        public override double BidPrice { get; set; }
        
        [JsonIgnore]
        public override double BidSize { get; set; } = 0;
        
        [JsonPropertyName("askPriceInDouble")]
        public override double AskPrice { get; set; }
        
        [JsonIgnore]
        public override double AskSize { get; set; } = 0;
        
        [JsonPropertyName("lastPriceInDouble")]
        public override double LastPrice { get; set; }
        
        [JsonIgnore]
        public override double LastSize { get; set; } = 0;

        [JsonPropertyName("highPriceInDouble")]
        public override double HighPrice { get; set; }

        [JsonPropertyName("lowPriceInDouble")]
        public override double LowPrice { get; set; }

        [JsonPropertyName("closePriceInDouble")]
        public override double ClosePrice { get; set; }

        [JsonPropertyName("openPriceInDouble")]
        public override double OpenPrice { get; set; }

        [JsonPropertyName("changeInDouble")]
        public double Change { get; set; }

        [JsonPropertyName("openInterest")]
        public int OpenInterest { get; set; }

        [JsonPropertyName("futurePriceFormat")]
        public string? FuturePriceFormat { get; set; }

        [JsonPropertyName("futureTradingHours")]
        public string? FutureTradingHours { get; set; }

        [JsonPropertyName("futureIsTradable")]
        public bool FutureIsTradable { get; set; }

        [JsonPropertyName("futureMultiplier")]
        public double FutureMultiplier { get; set; }

        [JsonPropertyName("futureIsActive")]
        public bool FutureIsActive { get; set; }

        [JsonPropertyName("futureSettlementPrice")]
        public double FutureSettlementPrice { get; set; }

        [JsonPropertyName("futureActiveSymbol")]
        public string? FutureActiveSymbol { get; set; }

        [JsonPropertyName("futureExpirationDate")]
        public long FutureExpirationDate { get; set; }
    }
}

