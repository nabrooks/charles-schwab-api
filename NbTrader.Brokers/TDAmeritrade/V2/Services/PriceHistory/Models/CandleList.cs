using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory.Models
{
    public class CandleList
    {
        [JsonProperty("candles")]
        public IList<Candle>? Candles{ get; set; }

        [JsonProperty("empty")]
        public bool Empty { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
    }
}