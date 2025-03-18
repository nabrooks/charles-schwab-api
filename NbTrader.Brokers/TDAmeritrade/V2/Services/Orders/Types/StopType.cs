using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types
{
    public enum StopType
    {
        [EnumMember(Value = "STANDARD")]
        STANDARD,
        [EnumMember(Value = "BID")]
        BID,
        [EnumMember(Value = "ASK")]
        ASK,
        [EnumMember(Value = "LAST")]
        LAST,
        [EnumMember(Value = "MARK")]
        MARK,
    }
}