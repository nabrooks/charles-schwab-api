#nullable enable

using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Models
{
    public partial class Instrument
    {
        [JsonProperty("assetType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual AssetType AssetType{ get; set; }

        [JsonProperty("cusip")]
        public virtual string? Cusip { get; set; }

        [JsonProperty("symbol")]
        public virtual string? Symbol { get; set; }

        [JsonProperty("description")]
        public virtual string? Description { get; set; }
    }
}
