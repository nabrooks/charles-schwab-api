using NbTrader.Brokers.TDAmeritrade.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.Principals.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.Principals
{
    public  interface IUserPrincipalsService
    {
        Task<TDPrincipal> GetPrincipals(params PrincipalType[] fields);
    }
}
