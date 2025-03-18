using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NbTrader.Utility.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.Models
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum MarketTypes
    {
        [EnumMember(Value = "BOND")] BOND,
        [EnumMember(Value = "EQUITY")] EQUITY,
        [EnumMember(Value = "ETF")] ETF,
        [EnumMember(Value = "FOREX")] FOREX,
        [EnumMember(Value = "FUTURE")] FUTURE,
        [EnumMember(Value = "FUTURE_OPTION")] FUTURE_OPTION,
        [EnumMember(Value = "INDEX")] INDEX,
        [EnumMember(Value = "INDICAT")] INDICAT,
        [EnumMember(Value = "MUTUAL_FUND")] MUTUAL_FUND,
        [EnumMember(Value = "OPTION")] OPTION,
        [EnumMember(Value = "UNKNOWN")] UNKNOWN
    }

    public class TDMarketHour
    {
        public DateTime date { get; set; }
        public string? marketType { get; set; }
        public bool isOpen { get; set; }
    }
}
