using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Principals.Models
{
    public class Principal
    {
        public string? authToken { get; set; }
        public string? userId { get; set; }
        public string? userCdDomainId { get; set; }
        public string? primaryAccountId { get; set; }
        public string? lastLoginTime { get; set; }
        public string? tokenExpirationTime { get; set; }
        public string? loginTime { get; set; }
        public string? accessLevel { get; set; }
        public bool stalePassword { get; set; }
        public StreamerInfo? streamerInfo { get; set; }
        public string? professionalStatus { get; set; }
        public QuotesAvailable? quotes { get; set; }
        public StreamerSubscriptionKeys? streamerSubscriptionKeys { get; set; }
    }

    [Serializable]
    public class StreamerInfo
    {
        public string? streamerBinaryUrl { get; set; }
        public string? streamerSocketUrl { get; set; }
        public string? token { get; set; }
        public string? tokenTimestamp { get; set; }
        public string? userGroup { get; set; }
        public string? accessLevel { get; set; }
        public string? acl { get; set; }
        public string? appId { get; set; }
    }
    [Serializable]
    public class QuotesAvailable
    {
        public bool isNyseDelayed { get; set; }
        public bool isNasdaqDelayed { get; set; }
        public bool isOpraDelayed { get; set; }
        public bool isAmexDelayed { get; set; }
        public bool isCmeDelayed { get; set; }
        public bool isIceDelayed { get; set; }
        public bool isForexDelayed { get; set; }
    }
    [Serializable]
    public class StreamerSubscriptionKeys
    {
        public List<Key>? keys { get; set; }
    }
    [Serializable]
    public class Key
    {
        public string? key { get; set; }
    }
}

