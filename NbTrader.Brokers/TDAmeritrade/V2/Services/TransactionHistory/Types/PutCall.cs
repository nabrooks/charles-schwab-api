using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types
{
    public enum PutCall
    {
        [EnumMember(Value = "PUT")]
        PUT,
        [EnumMember(Value = "CALL")]
        CALL,
    }
}