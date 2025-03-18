namespace NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient
{
    public class Http : IHttp
    {
        private static readonly System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();

        public System.Net.Http.HttpClient Client => _client;

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            return await SendAsync(httpRequestMessage, CancellationToken.None);
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken)
        {
                var result = await _client.SendAsync(httpRequestMessage, cancellationToken);
                return result;
        }

        public async Task<string> ReadAsStringAsync(HttpResponseMessage httpRequestMessage)
        {
            var result = await httpRequestMessage.Content.ReadAsStringAsync();
            return result;
        }
    }
}
