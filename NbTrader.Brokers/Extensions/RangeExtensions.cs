namespace NbTrader.Brokers.Extensions
{
    public static class RangeExtensions
    {
        public static CustomIntEnumerator GetEnumerator(this Range range) => new CustomIntEnumerator(range);
    }

    public ref struct CustomIntEnumerator
    {
        private int _current;
        private readonly int _end;

        public CustomIntEnumerator(Range range)
        {
            if (range.End.IsFromEnd)
            {
                throw new NotSupportedException();
            }
            _current = range.Start.Value - 1;
            _end = range.End.Value;
        }

        public int Current => _current;

        public bool MoveNext()
        {
            _current++;
            return _current <= _end;
        }
    }
}
