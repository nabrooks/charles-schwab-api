using System.Text.Json.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes.Models
{
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
}
