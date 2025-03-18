using NbTrader.Utility.Web.Server;

namespace NbTrader.Brokers.TDAmeritrade.Utilities
{
    /// <summary>
    /// Created this singleton to assert multiple instances of clients use the same redirect url api
    /// hosted by the same server to avoid creating clashing api endpoint instances
    /// </summary>
    public class TDRedirectServer : IDisposable
    {
        private static readonly Lazy<TDRedirectServer> lazy = new Lazy<TDRedirectServer>(() => new TDRedirectServer());
        private string _redirectUrl;
        private int _redirectUrlPort;
        private string _redirectUrlRoot;
        private string _redirectUrlRoute;
                
        private Server _server;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <exception cref="Exception"></exception>
        private TDRedirectServer() 
        {
            _redirectUrlRoot = "localhost";
            _redirectUrlPort = 44398;
            _redirectUrlRoute = "/authentication/redirect";
            _redirectUrl = $"https://{_redirectUrlRoot}:{_redirectUrlPort}{_redirectUrlRoute}";
            _server = new Server(_redirectUrlRoot, _redirectUrlPort, true, (ctx) => throw new Exception(""));
            _server.Start();
        }
        
        /// <summary>
        /// Instance property
        /// </summary>
        public static TDRedirectServer Instance { get { return lazy.Value; } }

        /// <summary>
        /// Path to the redirect Url
        /// </summary>
        public string RedirectUrl => _redirectUrl;


        /// <summary>
        /// Adds a functional handler upon pinging of api enpoint
        /// </summary>
        /// <param name="handler">The function to execute (are executed in sequence added)</param>
        public void AddHandler(Func<HttpContext,Task> handler)
        {
            _server.Routes.Static.Add(Utility.Web.Server.HttpMethod.GET, $"{_redirectUrlRoute}/", handler);
        }

        public void RemoveHandler()
        {
            _server.Routes.Static.Remove(Utility.Web.Server.HttpMethod.GET, $"{_redirectUrlRoute}/");
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}