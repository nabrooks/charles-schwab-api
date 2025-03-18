using System.Net;
using NbTrader.Brokers.TDAmeritrade.V2.Exceptions;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpClient;
using NbTrader.Brokers.TDAmeritrade.V2.Network.HttpRequest;
using NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services
{
    public abstract class AbstractService
    {
        protected readonly IHttpRequestMessageService httpRequestMessageService;

        protected readonly IHttp http;

        protected AbstractService(
            IHttp http,
            IHttpRequestMessageService httpRequestMessageService)
        {
            this.httpRequestMessageService = httpRequestMessageService;
            this.http = http;
        }

        private async Task<HttpResponseMessage> SendHttpRequestMessageAsync(
            HttpMethod httpMethod,
            string uri,
            string? content = null)
        {
            var httpRequestMessage = content == null
                ? httpRequestMessageService.CreateHttpRequestMessage(httpMethod, uri)
                : httpRequestMessageService.CreateHttpRequestMessage(httpMethod, uri, content);

            var httpResponseMessage = await http.SendAsync(httpRequestMessage).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine(httpResponseMessage);

                return httpResponseMessage;
            }

            var contentBody = await http.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);

            string? errorMessage;

            try
            {
                var jsonMsg = JsonConfig.DeserializeObject<TDAmeritradeErrorMessage>(contentBody);
                errorMessage = jsonMsg?.Message;
            }
            catch
            {
                errorMessage = contentBody;
            }

            var ex = new TDAmeritradeHttpException(errorMessage, httpResponseMessage.StatusCode, httpRequestMessage, httpResponseMessage, new EndPoint(httpMethod, uri, content));

            Console.WriteLine($"REST request about to throw {ex.Message}");

            throw ex;
        }

        protected async Task<IList<IList<T>>> SendHttpRequestMessagePagedAsync<T>(
            HttpMethod httpMethod,
            string uri,
            string? content = null,
            int numberOfPages = 0)
        {
            Console.WriteLine($"REST {httpMethod} {uri} {content} {numberOfPages}");

            var pagedList = new List<IList<T>>();

            var httpResponseMessage = await SendHttpRequestMessageAsync(httpMethod, uri, content);
            var contentBody = await http.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);

            var firstPage = JsonConfig.DeserializeObject<IList<T>>(contentBody);
            firstPage ??= new List<T>();

            pagedList.Add(firstPage);

            if (!httpResponseMessage.Headers.TryGetValues("cb-after", out var firstPageAfterCursorId))
            {
                return pagedList;
            }

            var subsequentPages = await GetAllSubsequentPages<T>(uri, firstPageAfterCursorId.First(), numberOfPages);

            pagedList.AddRange(subsequentPages);

            return pagedList;
        }

        private async Task<IList<IList<T>>> GetAllSubsequentPages<T>(
            string uri,
            string firstPageAfterCursorId,
            int numberOfPages)
        {
            var pagedList = new List<IList<T>>();
            var subsequentPageAfterHeaderId = firstPageAfterCursorId;

            var runCount = numberOfPages == 0
                ? int.MaxValue
                : numberOfPages;

            while (runCount > 1)
            {
                Console.WriteLine($"REST {HttpMethod.Get} {uri} {subsequentPageAfterHeaderId}");

                var subsequentHttpResponseMessage = await SendHttpRequestMessageAsync(HttpMethod.Get, uri + $"&after={subsequentPageAfterHeaderId}").ConfigureAwait(false);
                if (!subsequentHttpResponseMessage.Headers.TryGetValues("cb-after", out var cursorHeaders))
                {
                    break;
                }

                subsequentPageAfterHeaderId = cursorHeaders.First();

                var subsequentContentBody = await http.ReadAsStringAsync(subsequentHttpResponseMessage).ConfigureAwait(false);
                var page = JsonConfig.DeserializeObject<IList<T>>(subsequentContentBody);
                page ??= new List<T>();

                pagedList.Add(page);

                runCount--;
            }

            return pagedList;
        }

        protected async Task<T> SendServiceCall<T>(
            HttpMethod httpMethod,
            string uri,
            string? content = null)
        {
            Console.WriteLine($"REST {httpMethod} {uri} {content}");

            Console.WriteLine(uri);
            Console.WriteLine(content);
            var httpResponseMessage = await SendHttpRequestMessageAsync(httpMethod, uri, content);
            var contentBody = await http.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            Console.WriteLine(contentBody);

            if (typeof(T) == typeof(string))
            {
                return (T)(object)contentBody;
            }

            return JsonConfig.DeserializeObject<T>(contentBody)!;
        }

        protected async Task<T> SendServiceCallWithEmbeddedAPIkey<T>(
            HttpMethod httpMethod,
            string uri,
            string? content = null)
        {
            Console.WriteLine($"REST {httpMethod} {uri} {content}");

            Console.WriteLine(uri);
            string apiKey = WebUtility.UrlEncode(this.httpRequestMessageService.ApiKey + "@AMER.OAUTHAP");
            string uriToPass = uri;
            if(uri[uri.Length - 1] != '?') {uriToPass = uriToPass + "&";}
            Console.WriteLine(uriToPass+ $"apikey={apiKey}");
            var httpResponseMessage = await SendHttpRequestMessageAsync(httpMethod, uriToPass + $"apikey={apiKey}", content);
            var contentBody = await http.ReadAsStringAsync(httpResponseMessage).ConfigureAwait(false);
            Console.WriteLine(contentBody);
            if (typeof(T) == typeof(string))
            {
                return (T)(object)contentBody;
            }

            return JsonConfig.DeserializeObject<T>(contentBody)!;
        }
    }
}
