using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types
{
    public enum PositionEffect
    {
        [EnumMember(Value = "OPENING")]
        OPENING,
        [EnumMember(Value = "CLOSING")]
        CLOSING,
        [EnumMember(Value = "AUTOMATIC")]
        AUTOMATIC,
    }
}