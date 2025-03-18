namespace NbTrader.Brokers.Extensions;

public static class DoubleExtensions
{    
    private const double DefaultDoubleEpsilon = 1e-10;

    /// <summary>
    /// Determines if a double value is positive infinity
    /// </summary>
    public static bool IsPositiveInfinity(this double value)
        => BitConverter.DoubleToInt64Bits(value) == 0x7FF0000000000000L;

    /// <summary>
    /// Determines if a double value is negative infinity
    /// </summary>
    public static bool IsNegativeInfinity(this double value)
        => BitConverter.DoubleToInt64Bits(value) == unchecked((long)0xFFF0000000000000L);
    
    /// <summary>
    /// Determines if a double value is infinite (positive or negative infinity)
    /// Implementation without using Double.IsInfinite
    /// </summary>
    public static bool IsInfinite(this double value)
    {
        const long infinityBits = 0x7FF0000000000000L;
        long bits = BitConverter.DoubleToInt64Bits(value);
        return (bits & 0x7FFFFFFFFFFFFFFFL) == infinityBits;
    }

    /// <summary>
    /// Rounds a double to a specified number of significant figures
    /// </summary>
    public static double ToSignificantFigures(this double value, int significantFigures)
    {
        if (value == 0.0) return 0.0;
        if (double.IsNaN(value) || value.IsInfinite()) return value;

        double scale = Math.Pow(10, significantFigures - 1 - Math.Floor(Math.Log10(Math.Abs(value))));
        return Math.Round(value * scale) / scale;
    }
        
    /// <summary>
    /// Determines if a double is effectively zero within a specified epsilon
    /// </summary>
    public static bool IsEffectivelyZero(this double value, double epsilon = DefaultDoubleEpsilon)
        => Math.Abs(value) < epsilon;

    /// <summary>
    /// Returns the percentage difference between two values
    /// </summary>
    public static double PercentageDifference(this double value1, double value2)
    {
        if (value1.IsEffectivelyZero() && value2.IsEffectivelyZero()) return 0;
        if (value1.IsEffectivelyZero() || value2.IsEffectivelyZero()) return 100;

        return Math.Abs((value1 - value2) / ((value1 + value2) / 2)) * 100;
    }
    
    /// <summary>
    /// Nullable double comparison
    /// </summary>
    public static bool NearEquals(this double? value1, double? value2, double epsilon = DefaultDoubleEpsilon)
    {
        return (value1, value2) switch
        {
            (null, null) => true,
            (null, _) or (_, null) => false,
            (_, _) => value1.Value.NearEquals(value2.Value, epsilon)
        };
    }
    
    /// <summary>
    /// Optimized method to determine if two double values are nearly equal.
    /// </summary>
    public static bool NearEquals(this double value1, double value2, double epsilon = DefaultDoubleEpsilon)
    {
        if (value1 == value2) 
            return true;
            
        if (double.IsNaN(value1) || double.IsNaN(value2))
            return false;

        double diff = Math.Abs(value1 - value2);

        if (diff < epsilon)
            return true;

        double largest = Math.Max(Math.Abs(value1), Math.Abs(value2));
            
        return diff <= largest * epsilon;
    }

    /// <summary>
    /// Maps a double value from one range to another
    /// </summary>
    public static double Map(this double value, double fromMin, double fromMax, double toMin, double toMax)
    {
        if (Math.Abs(fromMax - fromMin) < double.Epsilon) return toMin;
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    /// <summary>
    /// Safe division that handles division by zero by returning a default value
    /// </summary>
    public static double SafeDivide(this double dividend, double divisor, double defaultValue = 0)
        => divisor.IsEffectivelyZero() ? defaultValue : dividend / divisor;

    /// <summary>
    /// Determines if a number is effectively an integer (has no decimal component)
    /// </summary>
    public static bool IsEffectivelyInteger(this double value, double epsilon = DefaultDoubleEpsilon)
        => Math.Abs(value - Math.Round(value)) < epsilon;
}