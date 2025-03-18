using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders.Types
{
    public enum CashEquivalentType
    {
        [EnumMember(Value = "SAVINGS")]
        SAVINGS,
        [EnumMember(Value = "MONEY_MARKET_FUND")]
        MONEY_MARKET_FUND,
    }
}