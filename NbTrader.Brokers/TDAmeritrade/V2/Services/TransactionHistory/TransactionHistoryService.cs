using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory
{
    public class TransactionHistoryService : AbstractService, ITransactionHistoryService
    {
        //private readonly QueryBuilder queryBuilder;

        public TransactionHistoryService(
            IHttp httpClient,
            IHttpRequestMessageService httpRequestMessageService)//,
            //QueryBuilder queryBuilder)
                : base(httpClient, httpRequestMessageService)
        {
            //this.queryBuilder = queryBuilder;
        }

        public async Task<IList<Transaction>> GetTransactionsByQueryAsync(
            string accountID, 
            TransactionType? type = null, 
            string? symbol = null, 
            DateTime? startDate = null, 
            DateTime? endDate = null)
            {
                string uri = $"/accounts/{accountID}/transactions?";//fromEnteredTime={from.ToString("yyy-MM-dd")}&toEnteredTime={to.ToString("yyyy-MM-dd")}";
                if(type != null)
                {
                    uri += $"type={type.ToString()}";
                }
                else
                {
                    uri += $"type={TransactionType.ALL}";
                }
                if(symbol != null && String.IsNullOrEmpty(symbol) == false)
                {
                    uri += $"&symbol={symbol}";
                }
                if(startDate != null)
                {
                    uri += $"&startDate={((DateTime)startDate).ToString("yyyy-MM-dd")}";
                    if(endDate != null)
                    {
                        uri += $"&endDate={((DateTime)endDate).ToString("yyyy-MM-dd")}";
                    }
                }
                
                return await SendServiceCall<IList<Transaction>>(HttpMethod.Get, uri);
            }

        public async Task<Transaction> GetTransactionAsync(string accountID, string transactionID)
        {
            string uri = $"/accounts/{accountID}/transactions/{transactionID}";//fromEnteredTime={from.ToString("yyy-MM-dd")}&toEnteredTime={to.ToString("yyyy-MM-dd")}";

            return await SendServiceCall<Transaction>(HttpMethod.Get, uri);
        }
    }
}