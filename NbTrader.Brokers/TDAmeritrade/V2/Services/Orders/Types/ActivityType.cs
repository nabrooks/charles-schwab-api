using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types
{
    public enum ActivityType
    {
        [EnumMember(Value = "EXECUTION")]
        EXECUTION,
        [EnumMember(Value = "ORDER_ACTION")]
        ORDER_ACTION,
    }
}