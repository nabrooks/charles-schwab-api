using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Models
{
    public class OrderActivity
    {
        [JsonProperty("ActivityType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual ActivityType DeliverableUnits { get; set; }
    }
}