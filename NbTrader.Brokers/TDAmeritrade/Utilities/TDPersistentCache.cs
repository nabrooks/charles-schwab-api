namespace NbTrader.Brokers.TDAmeritrade.Utilities
{

    /// <summary>
    /// Abstraction for saving persistent data
    /// </summary>
    internal interface ITdPersistentCache
    {
        void Save(string key, string value);
        string Load(string key);
    }
}
