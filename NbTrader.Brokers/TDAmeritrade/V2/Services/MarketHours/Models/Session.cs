using Newtonsoft.Json;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours.Models
{
    // public class SessionHours
    // {
    //     [JsonProperty("regularMarket")]
    //     public IList<SessionStartEnd> RegularMarketList { get; set; }
    //     [JsonProperty("preMarket")]
    //     public IList<SessionStartEnd> PreMarketList { get; set; }
    //     [JsonProperty("postMarket")]
    //     public IList<SessionStartEnd> PostMarketList { get; set; }

    //     [JsonIgnore]
    //     public DateTime RegularMarketStart {get{return RegularMarketList[0].Start;}}
    //     [JsonIgnore]
    //     public DateTime RegularMarketEnd {get{return RegularMarketList[0].End;}}
    //     [JsonIgnore]
    //     public DateTime PreMarketStart {get{return PreMarketList[0].Start;}}
    //     [JsonIgnore]
    //     public DateTime PreMarketEnd {get{return PreMarketList[0].End;}}
    //     [JsonIgnore]
    //     public DateTime PostMarketStart {get{return PostMarketList[0].Start;}}
    //     [JsonIgnore]
    //     public DateTime PostMarketEnd {get{return PostMarketList[0].End;}}
    // }

    public class Session//SessionStartEnd
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }
    }
}