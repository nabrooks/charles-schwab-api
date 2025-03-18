using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours.Types
{
    public enum SupportedMarketType
    {
        [EnumMember(Value = "EQUITY")]
        EQUITY,
        [EnumMember(Value = "OPTION")]
        OPTION,
    }
}