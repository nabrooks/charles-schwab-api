using LanguageExt;
using NbTrader.Brokers.TDAmeritrade.V3.Services.Accounts.Models;
using NbTrader.Utility;
using System.Net;
using System.Text.Json;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Services;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities;

namespace NbTrader.Brokers.TDAmeritrade.V3.Services.Accounts
{
    public class TDAccountService : AbstractService
    {
        public TDAccountService(IHttp http, IHttpRequestMessageService httpRequestMessageService)
            : base(http, httpRequestMessageService)
        { }

        public async Task<Seq<SecuritiesAccount>> GetAccountsAsync(bool includePositions = false, bool includeOrders = false)
        {
            string uri = $"/accounts";
            if (includePositions == true && includeOrders == true)
            {
                uri += "?fields=positions,orders";
            }
            else if (includePositions == true && includeOrders == false)
            {
                uri += "?fields=positions";
            }
            else if (includePositions == false && includeOrders == true)
            {
                uri += "?fields=orders";
            }
            string response = await SendServiceCall<string>(HttpMethod.Get, uri);

            var map = JsonConfig.DeserializeObject<IList<IDictionary<string, SecuritiesAccount>>>(response);
            IList<SecuritiesAccount> result = new List<SecuritiesAccount>();

            if (map is not null)
            {
                foreach (var dic in map)
                {
                    foreach (var entry in dic.Values)
                    {
                        result.Add(entry);
                    }
                }
            }
            return result.ToSeq();
        }

        public async Task<SecuritiesAccount> GetAccountAsync(string accountID, bool includePositions = false, bool includeOrders = false)
        {
            string uri = $"/accounts/{accountID}";
            if (includePositions == true && includeOrders == true)
            {
                uri += "?fields=positions,orders";
            }
            else if (includePositions == true && includeOrders == false)
            {
                uri += "?fields=positions";
            }
            else if (includePositions == false && includeOrders == true)
            {
                uri += "?fields=orders";
            }
            string response = await SendServiceCall<string>(HttpMethod.Get, uri);

            return JsonConfig.DeserializeObject<SecuritiesAccount>(response)!;
        }
    }
}
