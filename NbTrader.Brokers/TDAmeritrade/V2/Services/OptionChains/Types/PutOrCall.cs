using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.OptionChains.Types
{
    public enum PutOrCall
    {
        [EnumMember(Value = "PUT")]
        PUT,
        [EnumMember(Value = "CALL")]
        CALL,
    }
}