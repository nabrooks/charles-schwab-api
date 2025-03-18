using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Orders.Types;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Orders
{
    public class TDOrderService : AbstractService
    {
        public TDOrderService(
            IHttp httpClient,
            IHttpRequestMessageService httpRequestMessageService)
            : base(httpClient, httpRequestMessageService)
        { }
        public async Task<IList<Order>> GetOrdersByPathAsync(long AccountID, DateTime from, DateTime to, int? maxResults = null, Status status = Status.NOT_DEFINED)
        {
            //TODO: the From date should be within 60 days.. check for that.


            string uri = $"/accounts/{AccountID}/orders?fromEnteredTime={from.ToString("yyy-MM-dd")}&toEnteredTime={to.ToString("yyyy-MM-dd")}";
            if (maxResults != null)
            {
                uri += $"&maxResults={maxResults}";
            }
            if (status != Status.NOT_DEFINED)
            {
                uri += $"&status={status.ToString()}";
            }
            string response = await SendServiceCall<string>(HttpMethod.Get, uri);

            // //response = "[" + response.Substring(1,response.Length - 2) + "]";
            // response = JsonExtensions.parsedSearchResponse(response);
            // response = response.Remove(0, 1);
            // response = response.Remove(response.Length - 1, 1);

            // //Console.WriteLine(response);

            return JsonConfig.DeserializeObject<IList<Order>>(response)!;
        }

        public async Task<IList<Order>> GetOrdersByQueryAsync(DateTime from, DateTime to, int? maxResults = null, Status status = Status.NOT_DEFINED)
        {
            //TODO: the From date should be within 60 days.. check for that.
            string uri = $"/orders?fromEnteredTime={from.ToString("yyy-MM-dd")}&toEnteredTime={to.ToString("yyyy-MM-dd")}";
            if (maxResults != null)
            {
                uri += $"&maxResults={maxResults}";
            }
            if (status != Status.NOT_DEFINED)
            {
                uri += $"&status={status.ToString()}";
            }
            string response = await SendServiceCall<string>(HttpMethod.Get, uri);

            return JsonConfig.DeserializeObject<IList<Order>>(response)!;
        }

        public async Task<Order> GetOrderAsync(long accountID, long orderID)
        {
            string uri = $"/accounts/{accountID}/orders/{orderID}";
            string response = await SendServiceCall<string>(HttpMethod.Get, uri);
            return JsonConfig.DeserializeObject<Order>(response)!;
        }

        public async Task<string> PlaceOrderAsync(long AccountID, Order order)
        {
            string orderJson = JsonConfig.SerializeObject(order);

            return await SendServiceCall<string>(
                HttpMethod.Post,
                $"/accounts/{AccountID}/orders",
                orderJson);
        }

        public async Task<string> ReplaceOrderAsync(long AccountID, long orderIDtoReplace, Order order)
        {
            string orderJson = JsonConfig.SerializeObject(order);

            return await SendServiceCall<string>(
                HttpMethod.Put,
                $"/accounts/{AccountID}/orders/{orderIDtoReplace}",
                orderJson);
        }

        public async Task<string> CancelOrderAsync(long accountID, long orderID)
        {
            return await SendServiceCall<string>(
                HttpMethod.Delete,
                $"/accounts/{accountID}/orders/{orderID}");
        }

        public async Task<string> CreateSavedOrderAsync(long accountID, Order order)
        {
            string orderJson = JsonConfig.SerializeObject(order);

            return await SendServiceCall<string>(
                HttpMethod.Post,
                $"/accounts/{accountID}/savedorders",
                orderJson);
        }

        public async Task<string> DeleteSavedOrderAsync(long accountID, long savedOrderID)
        {

            return await SendServiceCall<string>(
                HttpMethod.Delete,
                $"/accounts/{accountID}/savedorders/{savedOrderID}");
        }

        public async Task<Order> GetSavedOrderAsync(long accountID, long savedOrderID)
        {
            string uri = $"/accounts/{accountID}/savedorders/{savedOrderID}";
            string response = await SendServiceCall<string>(HttpMethod.Get, uri);
            return JsonConfig.DeserializeObject<Order>(response)!;
        }

        public async Task<IList<Order>> GetSavedOrdersByPathAsync(long accountID)
        {
            //TODO: the From date should be within 60 days.. check for that.

            string uri = $"/accounts/{accountID}/savedorders";

            string response = await SendServiceCall<string>(HttpMethod.Get, uri);

            return JsonConfig.DeserializeObject<IList<Order>>(response)!;
        }

        public async Task<string> ReplaceSavedOrderAsync(long accountID, long replacedSavedOrderID, Order order)
        {
            string orderJson = JsonConfig.SerializeObject(order);

            return await SendServiceCall<string>(
                HttpMethod.Put,
                $"/accounts/{accountID}/savedorders/{replacedSavedOrderID}",
                orderJson);
        }
    }
}
