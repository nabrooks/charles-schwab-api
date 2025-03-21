using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types
{
    public enum AssetType
    {
        [EnumMember(Value = "EQUITY")]
        EQUITY,
        [EnumMember(Value = "MUTUAL_FUND")]
        MUTUAL_FUND,
        [EnumMember(Value = "OPTION")]
        OPTION,
        [EnumMember(Value = "FIXED_INCOME")]
        FIXED_INCOME,
        [EnumMember(Value = "CASH_EQUIVALENT")]
        CASH_EQUIVALENT,
    }
}