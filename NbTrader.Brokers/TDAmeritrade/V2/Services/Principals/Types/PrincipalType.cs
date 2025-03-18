using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Principals.Types
{
    [Serializable]
    public enum PrincipalType
    {
        [EnumMember(Value = "streamerSubscriptionKeys")]
        streamerSubscriptionKeys,
        [EnumMember(Value = "streamerConnectionInfo")]
        streamerConnectionInfo,
        [EnumMember(Value = "preferences")]
        preferences
    }
}
