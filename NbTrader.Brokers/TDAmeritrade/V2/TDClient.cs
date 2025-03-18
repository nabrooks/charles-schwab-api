using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Instruments;
using NbTrader.Brokers.TDAmeritrade.V2.Services.MarketHours;
using NbTrader.Brokers.TDAmeritrade.V2.Services.OptionChains;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders;
using NbTrader.Brokers.TDAmeritrade.V2.Services.PriceHistory;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Quotes;
using NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Clock;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Queries;
using HttpClient = NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient.Http;

namespace NbTrader.Brokers.TDAmeritrade.V2
{
    public class TDClient2
    {
        public TDClient2(TDAuthenticationService2 authenticator)
                : this(authenticator, new HttpClient())
        {
        }

        public TDClient2(
          TDAuthenticationService2 authenticator,
          IHttp httpClient)
        {
            var clock = new Clock();
            var httpRequestMessageService = new HttpRequestMessageService(authenticator, clock);
            var queryBuilder = new QueryBuilder();

            InstrumentsService = new InstrumentsService(httpClient, httpRequestMessageService);
            PriceHistoryService = new PriceHistoryService(httpClient, httpRequestMessageService);
            QuotesService = new QuotesService(httpClient, httpRequestMessageService);
            OrdersAndAccountsService = new OrdersService(httpClient, httpRequestMessageService);
            MarketHoursService = new MarketHoursService(httpClient, httpRequestMessageService);
            OptionChainsService = new OptionChainsService(httpClient, httpRequestMessageService);
            TransactionHistoryService = new TransactionHistoryService(httpClient, httpRequestMessageService);
        }
        public IInstrumentsService InstrumentsService { get; }
        public IPriceHistoryService PriceHistoryService { get; }
        public IQuotesService QuotesService { get; }
        public IOrdersAndAccountsService OrdersAndAccountsService { get; }
        public IMarketHoursService MarketHoursService { get; }
        public IOptionChainsService OptionChainsService { get; }
        public ITransactionHistoryService TransactionHistoryService { get; }
    }
}
