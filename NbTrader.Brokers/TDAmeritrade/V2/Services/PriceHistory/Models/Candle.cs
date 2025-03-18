using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Extensions;
using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory.Models
{
    public class Candle
    {
        [JsonProperty("close")]
        public decimal Close { get; set; }

        [JsonProperty("datetime")]
        [JsonConverter(typeof(EpochTimestampConverter))]
        public DateTime Datetime { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("open")]
        public decimal Open { get; set; }

        [JsonProperty("volume")]
        public Int64 Volume { get; set; }

    }
}