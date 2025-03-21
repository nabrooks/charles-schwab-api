using System.Text;
using NbTrader.Brokers.TDAmeritrade.V2.Shared;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Clock;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Extensions;

namespace NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest
{
    public class HttpRequestMessageService : IHttpRequestMessageService, IDisposable
    {
        private TDToken? token;
        
        private readonly TDAuthenticationService2 authenticator;
        private readonly IClock clock;
        private readonly IDisposable tokenSub;

        public HttpRequestMessageService(TDAuthenticationService2 authenticator, IClock clock)
        {
            this.authenticator = authenticator;
            this.clock = clock;

            this.tokenSub = this.authenticator.TokenUpdated.Subscribe((token) =>
            {
                this.token = token;
            });
        }

        public HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod,
            string requestUri,
            string contentBody = "")
        {
            // var apiUri = sandBox
            //     ? ApiUris.ApiUriSandbox
            //     : ApiUris.ApiUri;
            var apiUri = ApiUris.ApiUri;
            //Console.WriteLine("api uri: " + apiUri);
            //Console.WriteLine("RequestURI: " + requestUri);
            Console.WriteLine("REACHED HERE");
            Console.WriteLine(new Uri(apiUri + requestUri).ToString());
            var requestMessage = new HttpRequestMessage(httpMethod, new Uri(apiUri + requestUri))
            {
                Content = contentBody == string.Empty
                    ? null
                    : new StringContent(contentBody, Encoding.UTF8, "application/json")
            };

            var timeStamp = clock.GetTime().ToTimeStamp();

            //if (authenticator == null)
            //{
            //    AddHeaders(requestMessage, /*null,*/ timeStamp, false);

            //    return requestMessage;
            //}

            //var signedSignature = authenticator.ComputeSignature(httpMethod, authenticator.UnsignedSignature, timeStamp, requestUri, contentBody);

            AddHeaders(requestMessage, /*signedSignature,*/ timeStamp);//, true);
            return requestMessage;
        }

        private void AddHeaders(
            HttpRequestMessage httpRequestMessage,
            /*string signedSignature,*/
            double timeStamp//,
            /*bool includeAuthentication*/)
        {
            httpRequestMessage.Headers.Add("User-Agent", "TDAmeritradeClient");

            // if (!includeAuthentication)
            // {
            //     return;
            // }

            //TODO...
            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token?.AccessToken);
            // httpRequestMessage.Headers.Add("CB-ACCESS-KEY", authenticator.ApiKey);
            // httpRequestMessage.Headers.Add("CB-ACCESS-TIMESTAMP", timeStamp.ToString("F0", CultureInfo.InvariantCulture));
            // httpRequestMessage.Headers.Add("CB-ACCESS-SIGN", signedSignature);
            // httpRequestMessage.Headers.Add("CB-ACCESS-PASSPHRASE", authenticator.Passphrase);
        }

        public void Dispose()
        {
            this.tokenSub.Dispose();
            this.authenticator.Dispose();
        }

        public string ApiKey => authenticator.AppConsumerKey;
    }
}
