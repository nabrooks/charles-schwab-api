namespace NbTrader.Brokers.Extensions
{
    public static class IntExtensions
    {
        public static CustomIntEnumerator GetEnumerator(this Int32 endValueNotInclusive) => new CustomIntEnumerator(0..endValueNotInclusive);
    }
}
