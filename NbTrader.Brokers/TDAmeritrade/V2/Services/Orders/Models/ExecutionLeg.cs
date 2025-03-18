using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Models
{
    public class ExecutionLeg
    {
 
        [JsonProperty("legId")]
        public Int64 LegId { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("mismarkedQuantity")]
        public decimal MismarkedQuantity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}