using LanguageExt.Common;
using NbTrader.Brokers.TDAmeritrade.V3.Services.Accounts;
using NbTrader.Brokers.TDAmeritrade.V3.Services.Instruments;
using NbTrader.Brokers.TDAmeritrade.V3.Services.Orders;
using NbTrader.Brokers.TDAmeritrade.V3.Services.Quotes;
using System;
using System.Net.Http.Headers;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Clock;
using static LanguageExt.Prelude;

namespace NbTrader.Brokers.TDAmeritrade.V3
{
    public class TDClient3
    {
        public TDClient3(TDAuthenticationService2 authenticator, IHttp? httpClient = null)
        {
            var clock = new Clock();
            var http = httpClient ?? new Http();
            var httpRequestMessageService = new HttpRequestMessageService(authenticator, clock);

            AccountService = new TDAccountService(http, httpRequestMessageService);
            QuoteService = new TDQuoteService(http, httpRequestMessageService);
            OrderService = new TDOrderService(http, httpRequestMessageService);
            InstrumentService = new TDInstrumentsService(http, httpRequestMessageService);
        }

        public TDAccountService AccountService { get; }
        public TDQuoteService QuoteService { get; }
        public TDOrderService OrderService { get; }
        public TDInstrumentsService InstrumentService { get; }
    }
}
