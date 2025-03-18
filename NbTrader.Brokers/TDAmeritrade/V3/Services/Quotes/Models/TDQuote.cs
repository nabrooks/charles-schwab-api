using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes.Models
{
    public abstract class TDQuote
    {
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("exchange")]
        public string? Exchange { get; set; }
        [JsonPropertyName("exchangeName")]
        public string? ExchangeName { get; set; }
        [JsonPropertyName("securityStatus")]
        public string? SecurityStatus { get; set; }
        [JsonPropertyName("mark")]
        public double? Mark { get; set; }
        [JsonPropertyName("tick")]
        public double? Tick { get; set; }
        [JsonPropertyName("tickAmount")]
        public double? TickAmount { get; set; }

        public abstract double BidPrice { get; set; }
        public abstract double BidSize { get; set; }

        public abstract double AskPrice { get; set; }
        public abstract double AskSize { get; set; }
        public abstract double LastPrice { get; set; }
        public abstract double LastSize { get; set; }

        public abstract double OpenPrice { get; set; }
        public abstract double HighPrice { get; set; }
        public abstract double LowPrice { get; set; }
        public abstract double ClosePrice { get; set; }
    }
}
