﻿using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Instruments.Models
{
    public class Fundamental
    {
        [JsonProperty("cusip")]
        public string? Cusip { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("exchange")]
        public string? Exchange { get; set; }

        [JsonProperty("assetType")]
        public string? AssetType { get; set; }

        [JsonProperty("fundamental")]
        public FundamentalData? FundamentalData { get; set; }
    }

}
