#nullable enable
using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Models
{

    public class Fees
    {
        [JsonProperty("rFee")]
        public decimal? RFee { get; set; }

        [JsonProperty("additionalFee")]
        public decimal? AdditionalFee { get; set; }

        [JsonProperty("cdscFee")]
        public decimal? CdscFee { get; set; }

        [JsonProperty("regFee")]
        public decimal? RegFee { get; set; }

        [JsonProperty("otherCharges")]
        public decimal? OtherCharges { get; set; }

        [JsonProperty("commission")]
        public decimal? Commission { get; set; }

        [JsonProperty("optRegFee")]
        public decimal? OptRegFee { get; set; }

        [JsonProperty("secFee")]
        public decimal? SecFee { get; set; }
    }
}