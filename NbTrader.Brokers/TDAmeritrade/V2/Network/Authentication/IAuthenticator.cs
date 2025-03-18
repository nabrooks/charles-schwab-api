namespace NbTrader.Brokers.TDAmeritrade.V2.Network.Authentication
{
    public interface IAuthenticator
    {
        Models.EASObject? easObject {get;set;}
        string client_ID {get;}
    }
}