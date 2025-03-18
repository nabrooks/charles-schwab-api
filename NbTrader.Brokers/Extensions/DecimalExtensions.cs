namespace NbTrader.Brokers.Extensions;

public static class DecimalExtensions
{       
    private const decimal DefaultDecimalEpsilon = 0.0000001m;

    /// <summary>
    /// Optimized method for decimal comparison.
    /// Since decimal is already precise, we only need absolute comparison.
    /// </summary>
    public static bool NearEquals(this decimal value1, decimal value2, decimal epsilon = DefaultDecimalEpsilon)
    {
        return value1 == value2 || Math.Abs(value1 - value2) < epsilon;
    }

    /// <summary>
    /// Nullable decimal comparison
    /// </summary>
    public static bool NearEquals(this decimal? value1, decimal? value2, decimal epsilon = DefaultDecimalEpsilon)
    {
        return (value1, value2) switch
        {
            (null, null) => true,
            (null, _) or (_, null) => false,
            (_, _) => value1.Value.NearEquals(value2.Value, epsilon)
        };
    }

    /// <summary>
    /// Rounds a decimal amount to the nearest specified interval
    /// </summary>
    public static decimal RoundToInterval(this decimal value, decimal interval)
    {
        if (interval <= 0) throw new ArgumentException("Interval must be positive", nameof(interval));
        return Math.Round(value / interval) * interval;
    }

    /// <summary>
    /// Maps a decimal value from one range to another
    /// </summary>
    public static decimal Map(this decimal value, decimal fromMin, decimal fromMax, decimal toMin, decimal toMax)
    {
        if (fromMax - fromMin == 0) return toMin;
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    /// <summary>
    /// Safe division that handles division by zero by returning a default value
    /// </summary>
    public static decimal SafeDivide(this decimal dividend, decimal divisor, decimal defaultValue = 0)
    {
        if (divisor == 0) return defaultValue;
        return dividend / divisor;
    }

    /// <summary>
    /// Truncates a decimal to a specified number of decimal places
    /// </summary>
    public static decimal TruncateToDecimals(this decimal value, int decimals)
    {
        if (decimals < 0) throw new ArgumentException("decimals must be >= 0", nameof(decimals));
        decimal factor = (decimal)Math.Pow(10, decimals);
        return Math.Truncate(value * factor) / factor;
    }
}