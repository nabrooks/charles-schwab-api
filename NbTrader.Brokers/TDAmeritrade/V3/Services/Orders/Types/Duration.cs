using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum Duration
    {
        [EnumMember(Value = "DAY")]
        DAY,
        [EnumMember(Value = "GOOD_TILL_CANCEL")]
        GOOD_TILL_CANCEL,
        [EnumMember(Value = "FILL_OR_KILL")]
        FILL_OR_KILL,
    }
}