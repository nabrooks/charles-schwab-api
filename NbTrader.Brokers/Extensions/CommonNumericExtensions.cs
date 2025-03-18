namespace NbTrader.Brokers.Extensions;

public static class CommonNumericExtensions
{
    /// <summary>
    /// Determines if a number is a power of 2
    /// </summary>
    public static bool IsPowerOfTwo(this int value)
        => value > 0 && (value & (value - 1)) == 0;

    /// <summary>
    /// Gets the next power of 2 greater than or equal to the input
    /// </summary>
    public static int NextPowerOfTwo(this int value)
    {
        if (value <= 0) return 1;
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        return value + 1;
    }

    /// <summary>
    /// Extension method to check if a number is prime
    /// </summary>
    public static bool IsPrime(this int value)
    {
        if (value <= 1) return false;
        if (value == 2) return true;
        if (value % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(value));
        for (int i = 3; i <= boundary; i += 2)
        {
            if (value % i == 0) return false;
        }
        return true;
    }
}