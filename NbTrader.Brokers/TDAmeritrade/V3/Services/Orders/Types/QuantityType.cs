using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum QuantityType
    {
        [EnumMember(Value = "ALL_SHARES")]
        ALL_SHARES,
        [EnumMember(Value = "DOLLARS")]
        DOLLARS,
        [EnumMember(Value = "SHARES")]
        SHARES,
    }
}