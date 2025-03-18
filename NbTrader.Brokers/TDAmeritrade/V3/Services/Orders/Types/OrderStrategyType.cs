using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum OrderStrategyType
    {
        [EnumMember(Value = "SINGLE")]
        SINGLE,
        [EnumMember(Value = "OCO")]
        OCO,
        [EnumMember(Value = "TRIGGER")]
        TRIGGER,
    }
}