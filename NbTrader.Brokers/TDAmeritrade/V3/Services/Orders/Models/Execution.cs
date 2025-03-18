using System;
using System.Collections.Generic;

using System.Globalization;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Models
{
    public class Execution : OrderActivity
    {
        // [JsonProperty("ActivityType")]
        // public virtual ActivityType DeliverableUnits { get; set; }

        [JsonProperty("executionType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionType ExecutionType { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("orderRemainingQuantity")]
        public decimal OrderRemainingQuantity { get; set; }

        [JsonProperty("executionLegs")]
        public IList<ExecutionLeg>? ExecutionLegs { get; set; }
    }
}