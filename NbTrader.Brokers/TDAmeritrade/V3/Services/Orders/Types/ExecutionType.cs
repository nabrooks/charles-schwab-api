using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum ExecutionType
    {
        [EnumMember(Value = "FILL")]
        FILL,
    }
}