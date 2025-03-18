using System.Text.Json.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes.Models
{
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
}
