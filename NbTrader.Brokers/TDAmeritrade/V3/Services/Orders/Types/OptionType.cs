using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum OptionType
    {
        [EnumMember(Value = "VANILLA")]
        VANILLA,
        [EnumMember(Value = "BINARY")]
        BINARY,
        [EnumMember(Value = "BARRIER")]
        BARRIER,
    }
}