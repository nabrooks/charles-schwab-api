using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum Session
    {
        [EnumMember(Value = "NORMAL")]
        NORMAL,
        [EnumMember(Value = "AM")]
        AM,
        [EnumMember(Value = "PM")]
        PM,
        [EnumMember(Value = "SEAMLESS")]
        SEAMLESS,
    }
}