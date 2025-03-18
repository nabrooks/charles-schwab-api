namespace NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest
{
    public interface IHttpRequestMessageService
    {
        HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod,
            string requestUri,
            string contentBody = "");

        public string ApiKey {get;}
    }
}
