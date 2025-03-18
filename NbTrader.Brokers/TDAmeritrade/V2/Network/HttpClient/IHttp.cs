namespace NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient
{
    public interface IHttp
    {
        public System.Net.Http.HttpClient Client { get; }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken);

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);

        public Task<string> ReadAsStringAsync(HttpResponseMessage httpRequestMessage);
    }
}
