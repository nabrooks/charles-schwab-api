namespace NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Queries
{
    public interface IQueryBuilder
    {
        string BuildQuery(params KeyValuePair<string, string>[] queryParameters);
    }
}