using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Instruments.Types
{
    public enum AssetType
    {
        [EnumMember(Value = "EQUITY")]
        EQUITY,
        [EnumMember(Value = "ETF")]
        ETF,
        [EnumMember(Value = "FOREX")]
        FOREX,
        [EnumMember(Value = "FUTURE")]
        FUTURE,
        [EnumMember(Value = "FUTURE_OPTION")]
        FUTURE_OPTION,
        [EnumMember(Value = "INDEX")]
        INDEX,
        [EnumMember(Value = "INDICATOR")]
        INDICATOR,
        [EnumMember(Value = "MUTUAL_FUND")]
        MUTUAL_FUND,
        [EnumMember(Value = "OPTION")]
        OPTION,
        [EnumMember(Value = "UKNOWN")]
        UNKNOWN,
        [EnumMember(Value = "BOND")]
        BOND,
    }
}
