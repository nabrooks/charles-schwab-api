using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum OptionPutOrCall
    {
        [EnumMember(Value = "PUT")]
        PUT,
        [EnumMember(Value = "CALL")]
        CALL,
    }
}