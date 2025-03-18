using NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours.Models
{
    public class Hours
    {
        [JsonProperty("category")]
        public string? Category { get; set; }

        [JsonProperty("date")]
        public string? Date { get; set; }
            
        [JsonProperty("exchange")]
        public string? Exchange { get; set; }

        [JsonProperty("isOpen")]
        public bool IsOpen { get; set; }

        [JsonProperty("marketType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public  MarketType MarketType { get; set; }

        [JsonProperty("product")]
        public string? Product { get; set; }

        [JsonProperty("productName")]
        public string? ProductName { get; set; }

        [JsonProperty("sessionHours")]
        //public SessionHours SessionHours { get; set; }
        public IDictionary<string, IList<Session>>? SessionHours {get; set;}
    }
}