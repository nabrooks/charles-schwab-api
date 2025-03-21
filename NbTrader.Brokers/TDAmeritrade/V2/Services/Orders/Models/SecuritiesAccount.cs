#nullable enable

using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Models
{
    public class SecuritiesAccount
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual AccountType Type { get; set; }

        [JsonProperty("accountId")]
        public string? AccountId { get; set; }

        [JsonProperty("roundTrips")]
        public Int64 RoundTrips { get; set; }

        [JsonProperty("isDayTrader")]
        public bool IsDayTrader { get; set; }

        [JsonProperty("isClosingOnlyRestricted")]
        public bool IsClosingOnlyRestricted { get; set; }

        [JsonProperty("positions")]
        public IList<Position>? Positions { get; set; }

        [JsonProperty("orderStrategies")]
        public IList<Order>? OrderStrategies { get; set; }

        [JsonProperty("initialBalances")]
        public InitialBalances? InitialBalances { get; set; }

        [JsonProperty("currentBalances")]
        public CurrentOrProjectedBalances? CurrentBalances { get; set; }

        [JsonProperty("projectedBalances")]
        public CurrentOrProjectedBalances? ProjectedBalances { get; set; }
    }
}