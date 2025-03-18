using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types
{
    public enum Instruction
    {
        [EnumMember(Value = "BUY")]
        BUY,
        [EnumMember(Value = "SELL")]
        SELL,
    }
}