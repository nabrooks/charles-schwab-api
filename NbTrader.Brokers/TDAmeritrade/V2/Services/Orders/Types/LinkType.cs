using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types
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