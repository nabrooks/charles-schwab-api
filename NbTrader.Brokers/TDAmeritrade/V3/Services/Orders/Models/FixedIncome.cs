using System;
using System.Collections.Generic;

using System.Globalization;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Models
{
    public class FixedIncome : Instrument
    {
        [JsonProperty("assetType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override AssetType AssetType{ get {return AssetType.FIXED_INCOME;} }

        [JsonProperty("maturityDate")]
        public DateTime MaturityDate { get; set; }

        [JsonProperty("variableRate")]
        public Decimal VariableRate { get; set; }

        [JsonProperty("factor")]
        public Decimal Factor { get; set; }
    }
}