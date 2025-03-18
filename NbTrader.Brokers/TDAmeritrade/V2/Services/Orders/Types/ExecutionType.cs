using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types
{
    public enum ExecutionType
    {
        [EnumMember(Value = "FILL")]
        FILL,
    }
}