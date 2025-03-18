using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Accounts.Types
{
    public enum AccountType
    {
        [EnumMember(Value = "CASH")]
        CASH,
        [EnumMember(Value = "MARGIN")]
        MARGIN,
    }
}