using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory.Types
{
    public enum FrequencyType
    {
        [EnumMember(Value = "minute")]
        minute,
        [EnumMember(Value = "daily")]
        daily,
        [EnumMember(Value = "weekly")]
        weekly,
        [EnumMember(Value = "monthly")]
        monthly,
    }
}