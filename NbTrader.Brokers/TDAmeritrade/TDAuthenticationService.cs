using LanguageExt;
using LanguageExt.Common;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Authentication;
using System.Text.Json;
using NbTrader.Brokers.TDAmeritrade.Utilities;
using static LanguageExt.Prelude;

namespace NbTrader.Brokers.TDAmeritrade
{
    public class TDAuthenticationService2 : IDisposable
    {
        private const string BaseAuthenticationUrl = "https://auth.TDAmeritrade.com";
        private const string AuthenticationEndpoint = BaseAuthenticationUrl + "/auth?";
        private const string TokenRefreshEndpoint = "https://api.TDAmeritrade.com/v1/oauth2/token";
        private const string AuthenticationTokenJsonKey = "TDAuthenticationToken";
        
        private readonly HttpClient _http;
        private readonly TDCache _cache;
        private readonly string _appConsumerKey;
        private BehaviorSubject<TDToken> _tokenSubject;
        private IDisposable _tokenStorageSubscription;
        private IDisposable _tokenValidityPollerSubscription;
        private TDToken _token;

        public TDAuthenticationService2(string appConsumerKey) : this(appConsumerKey, new HttpClient(), new TDCache()) { }
        
        public TDAuthenticationService2(string appConsumerKey, HttpClient client) : this(appConsumerKey, client, new TDCache()) { }

        public TDAuthenticationService2(string appConsumerKey, HttpClient client, TDCache cache)
        {
            _http = client;
            _cache = cache;
            _appConsumerKey = appConsumerKey;

            // Try to read stored token in file.  If it doesnt work, then set to new TDToken instance
            _token = _cache
                .Load(AuthenticationTokenJsonKey)
                .Match(
                    Right: (json) =>
                    {
                        if (String.IsNullOrWhiteSpace(json))
                            return new TDToken();

                        var result = JsonSerializer.Deserialize<TDToken>(json);
                        if (result is not null) return result;
                        return new TDToken();
                    },
                    Left: (ex) => new TDToken());

            // Evaluate validity of current token, if it isnt valid, get a new one, if it is but needs a refresh, then do so
            if (_token.IsValid == false)
                _tokenSubject = new BehaviorSubject<TDToken>(SignIn().GetAwaiter().GetResult());
            else if (_token.NeedsRefresh && _token.CanRefresh)
                _tokenSubject = new BehaviorSubject<TDToken>(RefreshToken(_token).GetAwaiter().GetResult());
            else
                _tokenSubject = new BehaviorSubject<TDToken>(_token!);

            // subscribe internally to token updates for storage
            _tokenStorageSubscription = _tokenSubject.Subscribe((token) =>
            {
                _token = token;
                _cache.Save(AuthenticationTokenJsonKey, JsonSerializer.Serialize(token));
            });

            // Setup poller to evaluate if token needs a refresh or is invalid.

            _tokenValidityPollerSubscription = Observable
                //.Timer(TimeSpan.Zero, TimeSpan.FromMinutes(2))
                .Interval(TimeSpan.FromMinutes(10))
                .Subscribe((pollCounter) =>
                {
                    if (_tokenSubject.Value.IsValid == false)
                        _tokenSubject.OnNext(SignIn().GetAwaiter().GetResult());
                    else if (_tokenSubject.Value.NeedsRefresh && _tokenSubject.Value.CanRefresh)
                        _tokenSubject.OnNext(RefreshToken(_tokenSubject.Value).GetAwaiter().GetResult());
                });
        }

        public TDToken Token => _token;

        public IObservable<TDToken> TokenUpdated => _tokenSubject;

        public string AppConsumerKey => _appConsumerKey;

        private async Task<TDToken> SignIn()
        {
            var authorizationUrl = $@"{AuthenticationEndpoint}response_type=code&redirect_uri={TDRedirectServer.Instance.RedirectUrl}&client_id={this._appConsumerKey}@AMER.OAUTHAP";
            var tcsToken = new TaskCompletionSource<TDToken>();

            TDRedirectServer.Instance.AddHandler(async ctx => {
                var queryString = ctx.Request.Query.Querystring;
                var authorizationCodeDecrypted = WebUtility.UrlDecode(queryString.Remove(0, 5));
                var dict = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "access_type", "offline" },
                    { "code", authorizationCodeDecrypted },
                    { "client_id", _appConsumerKey + "@AMER.OAUTHAP" },
                    { "redirect_uri", TDRedirectServer.Instance.RedirectUrl }
                };

                var req = new HttpRequestMessage(HttpMethod.Post, TokenRefreshEndpoint) { Content = new FormUrlEncodedContent(dict) };
                var res = await _http.SendAsync(req);

                if (res.StatusCode != HttpStatusCode.OK)
                    throw (new AuthenticationException($"{res.StatusCode} {res.ReasonPhrase}"));

                var json = await res.Content.ReadAsStringAsync();
                var token = new TDToken();

                using (JsonDocument jsonDoc = JsonDocument.Parse(json))
                {
                    var now = DateTime.Now;
                    var jRoot = jsonDoc.RootElement;

                    token.AccessToken = jRoot.GetProperty("access_token").GetString();
                    token.RefreshToken = jRoot.GetProperty("refresh_token").GetString();
                    token.Scope = jRoot.GetProperty("scope").GetString();
                    token.Expiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("expires_in").GetDouble());
                    token.RefreshTokenExpiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("refresh_token_expires_in").GetDouble());
                    token.TokenType = jRoot.GetProperty("token_type").GetString();
                }

                tcsToken.SetResult(token);
            });

            Process? process = Process.Start(new ProcessStartInfo("cmd", $"/c start {authorizationUrl.Replace("&", "^&")}") { CreateNoWindow = true });

            TDToken result = await tcsToken.Task;

            if (process is null)
                throw new NullReferenceException("Process to open web browser could not be started");

            process.Kill();
            process.Close();
            process.Dispose();
            return result;
        }

        private async Task<TDToken> RefreshToken(TDToken oldToken)
        {
            if (string.IsNullOrWhiteSpace(oldToken.RefreshToken))
                throw new NullReferenceException("Authentication token object and refresh token string must not be null in order to refresh");

            var dict = new Dictionary<string, string>()
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", oldToken.RefreshToken },
                    { "client_id", _appConsumerKey + "@AMER.OAUTHAP" }
                };
            var req = new HttpRequestMessage(HttpMethod.Post, TokenRefreshEndpoint) { Content = new FormUrlEncodedContent(dict) };
            var res = await _http.SendAsync(req);

            if (res.StatusCode != HttpStatusCode.OK)
                throw (new AuthenticationException($"{res.StatusCode} {res.ReasonPhrase}"));

            var json = await res.Content.ReadAsStringAsync();
            using (JsonDocument jsonDoc = JsonDocument.Parse(json))
            {
                var now = DateTime.Now;
                var jRoot = jsonDoc.RootElement;
                var token = new TDToken();

                token.AccessToken = jRoot.GetProperty("access_token").GetString();
                token.RefreshToken = oldToken.RefreshToken;// jRoot.GetProperty("refresh_token").GetString();
                token.Scope = jRoot.GetProperty("scope").GetString();
                token.Expiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("expires_in").GetDouble());
                token.RefreshTokenExpiration = oldToken.RefreshTokenExpiration;//now + TimeSpan.FromSeconds(jRoot.GetProperty("refresh_token_expires_in").GetDouble());
                token.TokenType = jRoot.GetProperty("token_type").GetString();
                return token;
            }
        }

        private async Task<TDToken> PostToken(string path, IEnumerable<KeyValuePair<string, string>> args)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, path) { Content = new FormUrlEncodedContent(args) };
            var res = await _http.SendAsync(req);

            if (res.StatusCode != HttpStatusCode.OK)
                throw (new AuthenticationException($"{res.StatusCode} {res.ReasonPhrase}"));

            var json = await res.Content.ReadAsStringAsync();
            using (JsonDocument jsonDoc = JsonDocument.Parse(json))
            {
                var now = DateTime.Now;
                var jRoot = jsonDoc.RootElement;
                var token = new TDToken();

                token.AccessToken = jRoot.GetProperty("access_token").GetString();
                token.RefreshToken = jRoot.GetProperty("refresh_token").GetString();
                token.Scope = jRoot.GetProperty("scope").GetString();
                token.Expiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("expires_in").GetDouble());
                token.RefreshTokenExpiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("refresh_token_expires_in").GetDouble());
                token.TokenType = jRoot.GetProperty("token_type").GetString();
                return token;
            }

        }

        public void Dispose()
        {
            _http.Dispose();
            _tokenStorageSubscription.Dispose();
            _tokenValidityPollerSubscription.Dispose();
            TDRedirectServer.Instance.Dispose();
        }
    }

    public class TDAuthenticationService : IDisposable
    {
        private const string BaseAuthenticationUrl = "https://auth.TDAmeritrade.com";
        private const string AuthenticationEndpoint = BaseAuthenticationUrl + "/auth?";
        private const string TokenRefreshEndpoint = "https://api.TDAmeritrade.com/v1/oauth2/token";
        private const string AuthenticationTokenJsonKey = "TDAuthenticationToken";

        private readonly TDCache _cache;
        private readonly string _appConsumerKey;
        private BehaviorSubject<EitherAsync<Error, TDToken>> _tokenSubject;
        private IDisposable _tokenStorageSubscription;
        private IDisposable _tokenValidityPollerSubscription;

        public TDAuthenticationService(string appConsumerKey)
        {
            _cache = new TDCache();
            _appConsumerKey = appConsumerKey;

            // Try to read stored token in file.  If it doesnt work, then set to new TDToken instance
            var oldToken = _cache
                .Load(AuthenticationTokenJsonKey)
                .Match(
                    Right: (json) =>
                    {
                        if (String.IsNullOrWhiteSpace(json))
                            return new TDToken();

                        var result = JsonSerializer.Deserialize<TDToken>(json);
                        if (result is not null) return result;
                        return new TDToken();
                    },
                    Left: (ex) => new TDToken());

            // Evaluate validity of current token, if it isnt valid, get a new one, if it is but needs a refresh, then do so
            if (oldToken.IsValid == false)
                _tokenSubject = new BehaviorSubject<EitherAsync<Error, TDToken>>(SignIn());
            else if (oldToken.NeedsRefresh && oldToken.CanRefresh)
                _tokenSubject = new BehaviorSubject<EitherAsync<Error, TDToken>>(RefreshToken(oldToken));
            else
                _tokenSubject = new BehaviorSubject<EitherAsync<Error, TDToken>>(oldToken!);
            //else
            //    _tokenSubject = new BehaviorSubject<EitherAsync<Error, TDToken>>(EitherAsync<Error, TDToken>.Left(Error.New("Error getting token from Td Api Service")));

            // subscribe internally to token updates for storage
            _tokenStorageSubscription = _tokenSubject.Subscribe((tokenEither) =>
            {
                Console.WriteLine("Token storage event posted");
                tokenEither.Match(
                    Right: (token) => _cache.Save(AuthenticationTokenJsonKey, JsonSerializer.Serialize(token)),
                    Left: (error) => _cache.Save(AuthenticationTokenJsonKey, JsonSerializer.Serialize(new TDToken())));
            });

            // Setup poller to evaluate if token needs a refresh or is invalid.

            _tokenValidityPollerSubscription = Observable
                //.Timer(TimeSpan.Zero, TimeSpan.FromMinutes(2))
                .Interval(TimeSpan.FromSeconds(10))
                .Subscribe(async (pollCounter) =>
                {
                    Console.WriteLine($"Token refresh eval {pollCounter} event posted");
                    await _tokenSubject.Value.Match(
                        Right: (token) =>
                        {
                            Console.WriteLine(token.ToString());

                            if (token.IsValid == false)
                                _tokenSubject.OnNext(SignIn());
                            else if (token.NeedsRefresh && token.CanRefresh)
                                _tokenSubject.OnNext(RefreshToken(token));
                        },
                        Left: (error) =>
                        {
                            _tokenSubject.OnNext(SignIn());
                        });
                });
        }

        public IObservable<EitherAsync<Error, TDToken>> TokenUpdated => _tokenSubject;

        public string AppConsumerKey => _appConsumerKey;

        private EitherAsync<Error, TDToken> SignIn()
        {
            var authorizationUrl = $@"{AuthenticationEndpoint}response_type=code&redirect_uri={TDRedirectServer.Instance.RedirectUrl}&client_id={this._appConsumerKey}@AMER.OAUTHAP";
            var tcsToken = new TaskCompletionSource<Either<Error, TDToken>>();

            TDRedirectServer.Instance.AddHandler(async ctx => {
                var queryString = ctx.Request.Query.Querystring;
                var authorizationCodeDecrypted = WebUtility.UrlDecode(queryString.Remove(0, 5));
                var dict = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "access_type", "offline" },
                    { "code", authorizationCodeDecrypted },
                    { "client_id", _appConsumerKey + "@AMER.OAUTHAP" },
                    { "redirect_uri", TDRedirectServer.Instance.RedirectUrl }
                };
                tcsToken.SetResult(await PostToken(TokenRefreshEndpoint, dict));
            });

            Process? process = Process.Start(new ProcessStartInfo("cmd", $"/c start {authorizationUrl.Replace("&", "^&")}") { CreateNoWindow = true });

            EitherAsync<Error, TDToken> result = tcsToken.Task.GetAwaiter().GetResult().ToAsync();

            if (process is null)
                throw new NullReferenceException("Process to open web browser could not be started");

            process.Kill();
            process.Close();
            process.Dispose();
            return result;
        }

        private EitherAsync<Error, TDToken> RefreshToken(TDToken oldToken)
        {

            if (string.IsNullOrWhiteSpace(oldToken.RefreshToken))
                throw new NullReferenceException("Authentication token object and refresh token string must not be null in order to refresh");

            var dict = new Dictionary<string, string>()
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", oldToken.RefreshToken },
                    { "client_id", _appConsumerKey + "@AMER.OAUTHAP" }
                };
            return PostToken(TokenRefreshEndpoint, dict);
        }

        private static EitherAsync<Error, TDToken> PostToken(string path, IEnumerable<KeyValuePair<string, string>> args)
        {
            return TryAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    var req = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, path) { Content = new FormUrlEncodedContent(args) };
                    var res = await client.SendAsync(req);

                    if (res.StatusCode != HttpStatusCode.OK)
                        throw (new AuthenticationException($"{res.StatusCode} {res.ReasonPhrase}"));

                    var json = await res.Content.ReadAsStringAsync();
                    return new TDToken(json);
                }
            }).ToEither();
        }

        public void Dispose()
        {
            _tokenStorageSubscription.Dispose();
            _tokenValidityPollerSubscription.Dispose();
            TDRedirectServer.Instance.Dispose();
        }
    }
}