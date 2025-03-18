namespace NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Clock
{
    public class Clock : IClock
    {
        public DateTime GetTime()
        {
            return DateTime.UtcNow;
        }
    }
}
