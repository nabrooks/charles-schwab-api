﻿using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments.Types;
using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Instruments.Models
{
    public partial class Instrument
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
        public AssetType AssetType { get; set; }
    }

}
