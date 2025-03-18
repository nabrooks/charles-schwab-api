using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Models
{
    public class OptionDeliverable
    {
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("deliverableUnits")]
        public decimal? DeliverableUnits { get; set; }

        [JsonProperty("currencyType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyType CurrencyType { get; set; }

        [JsonProperty("assetType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssetType AssetType { get; set; }
    }
}