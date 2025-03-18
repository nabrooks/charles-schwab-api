using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Models
{
    public class Bond
    {
        [JsonProperty("bondPrice")]
        public decimal? BondPrice { get; set; }

        [JsonProperty("cusip")]
        public string? Cusip { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("exchange")]
        public string? Exchange { get; set; }

        [JsonProperty("assetType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssetType AssetType { get; set; } = AssetType.BOND;
    }
}