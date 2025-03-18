using System.Text.Json.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes.Models
{
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
}
