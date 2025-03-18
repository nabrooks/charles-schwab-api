using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.OptionChains.Types
{
    public enum ContractTypeFilter
    {
        [EnumMember(Value = "PUT")] //Standard Contracts
        PUT,
        [EnumMember(Value = "CALL")] //Non-Standard Contracts
        CALL,
        [EnumMember(Value = "ALL")]
        ALL,
    }
}