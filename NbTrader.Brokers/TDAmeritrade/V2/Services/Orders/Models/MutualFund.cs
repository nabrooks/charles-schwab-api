using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Models
{
    public class MutualFund : Instrument
    {
        [JsonProperty("assetType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override AssetType AssetType{ get {return AssetType.MUTUAL_FUND;} }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MutualFundType Type { get; set; }
    }
}