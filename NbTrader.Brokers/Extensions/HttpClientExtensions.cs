using System.Web;

namespace NbTrader.Brokers.Extensions
{
    /// <summary>
    /// enum for the <see cref="TryRequest"/> convenience method
    /// </summary>
    public enum HttpRequestMethod
    {
        Get,
        Put,
        Post,
        Delete,
        Patch
    }

    public static class HttpClientExtensions
    {
        /// <summary>
        /// A convenience method for http api consumption
        /// </summary>
        /// <param name="token">The authentication token to use</param>
        /// <param name="requestUrl">The http api url</param>
        /// <param name="queryParameters">Any string query parameters to be embedded in the url</param>
        /// <param name="bodyParameters">Any body parameters to be json serialized and passed to api</param>
        /// <param name="method">The type of rest api method to call</param>
        /// <returns>An http response</returns>
        public static async Task<HttpResponseMessage> TryRequestAsync(this HttpClient httpClient, string authToken, string requestUrl, IEnumerable<KeyValuePair<string, string>> queryParameters, IEnumerable<KeyValuePair<string, string>> bodyParameters, HttpRequestMethod method)
        {
            bodyParameters = bodyParameters ?? new List<KeyValuePair<string,string>>();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

            HttpResponseMessage responseMessage = null;

            UriBuilder uriBuilder = new UriBuilder(requestUrl);
            if (queryParameters != null && queryParameters.Any() == true)
            {
                uriBuilder.Port = -1;
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                queryParameters.ForEach(kvp => query[kvp.Key] = kvp.Value);
                uriBuilder.Query = query.ToString();
            }
            var uri = uriBuilder.ToString();
            switch (method)
            {
                case HttpRequestMethod.Get:
                    responseMessage = await httpClient.GetAsync(new Uri(uri));
                    break;
                case HttpRequestMethod.Put:
                    responseMessage = await httpClient.PutAsync(new Uri(uri), new FormUrlEncodedContent(bodyParameters));
                    break;
                case HttpRequestMethod.Post:
                    responseMessage = await httpClient.PostAsync(new Uri(uri), new FormUrlEncodedContent(bodyParameters));
                    break;
                case HttpRequestMethod.Patch:
                    responseMessage = await httpClient.PatchAsync(new Uri(uri), new FormUrlEncodedContent(bodyParameters));
                    break;
                case HttpRequestMethod.Delete:
                    responseMessage = await httpClient.DeleteAsync(new Uri(uri));
                    break;
                default:
                    throw new HttpRequestException($"Api method {method} not recognized or implemented.");
            }
            return responseMessage;
        }
    }

}
