using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory.Types
{
    public enum PeriodType
    {
        [EnumMember(Value = "day")]
        day,
        [EnumMember(Value = "month")]
        month,
        [EnumMember(Value = "year")]
        year,
        [EnumMember(Value = "ytd")]
        ytd,
    }
}