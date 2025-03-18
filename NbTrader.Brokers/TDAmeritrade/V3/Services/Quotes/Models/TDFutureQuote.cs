using System.Text.Json.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes.Models
{
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
