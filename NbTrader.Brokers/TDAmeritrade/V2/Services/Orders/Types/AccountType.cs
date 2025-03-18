using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types
{
    public enum AccountType
    {
        [EnumMember(Value = "CASH")]
        CASH,
        [EnumMember(Value = "MARGIN")]
        MARGIN,
    }
}