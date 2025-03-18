using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum LinkType
    {
        [EnumMember(Value = "VALUE")]
        VALUE,
        [EnumMember(Value = "PERCENT")]
        PERCENT,
        [EnumMember(Value = "TICK")]
        TICK,
    }
}