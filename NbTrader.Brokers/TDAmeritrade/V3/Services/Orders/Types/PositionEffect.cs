using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
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