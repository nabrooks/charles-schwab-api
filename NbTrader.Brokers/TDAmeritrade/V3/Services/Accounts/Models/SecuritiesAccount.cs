using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Models;

#nullable enable

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Accounts.Models
{
    public class SecuritiesAccount
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual AccountType Type { get; set; }

        [JsonProperty("accountId")]
        public string? AccountId { get; set; }

        [JsonProperty("roundTrips")]
        public long RoundTrips { get; set; }

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