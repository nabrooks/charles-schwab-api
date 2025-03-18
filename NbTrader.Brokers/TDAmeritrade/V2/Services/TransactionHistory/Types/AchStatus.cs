using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types
{
    public enum AchStatus
    {
        [EnumMember(Value = "Approved")]
        APPROVED,
        [EnumMember(Value = "Rejected")]
        REJECTED,
        [EnumMember(Value = "Cancel")]
        CANCEL,
        [EnumMember(Value = "Error")]
        ERROR,
    }
}