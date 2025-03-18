namespace NbTrader.Brokers.Extensions;

public static class FloatExtensions
{
    private const float DefaultFloatEpsilon = 1e-6f;
        
    /// <summary>
    /// Checks if a number is between two values (inclusive)
    /// </summary>
    public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable<T>
        => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;

    /// <summary>
    /// Clamps a value between a minimum and maximum
    /// </summary>
    public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        => value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;

    /// <summary>
    /// Optimized method to determine if two float values are nearly equal.
    /// </summary>
    public static bool NearEquals(this float value1, float value2, float epsilon = DefaultFloatEpsilon)
    {
        if (value1 == value2)
            return true;

        if (float.IsNaN(value1) || float.IsNaN(value2))
            return false;

        float diff = Math.Abs(value1 - value2);

        if (diff < epsilon)
            return true;

        float largest = Math.Max(Math.Abs(value1), Math.Abs(value2));
        return diff <= largest * epsilon;
    }

    /// <summary>
    /// Determines if a float value is infinite (positive or negative infinity)
    /// Implementation without using Float.IsInfinite
    /// </summary>
    public static bool IsInfinite(this float value)
    {
        const int infinityBits = 0x7F800000;
        int bits = BitConverter.SingleToInt32Bits(value);
        return (bits & 0x7FFFFFFF) == infinityBits;
    }

    /// <summary>
    /// Determines if a float value is positive infinity
    /// </summary>
    public static bool IsPositiveInfinity(this float value)
        => BitConverter.SingleToInt32Bits(value) == 0x7F800000;

    /// <summary>
    /// Determines if a float value is negative infinity
    /// </summary>
    public static bool IsNegativeInfinity(this float value)
        => BitConverter.SingleToInt32Bits(value) == unchecked((int)0xFF800000);

    /// <summary>
    /// Determines if a float is effectively zero within a specified epsilon
    /// </summary>
    public static bool IsEffectivelyZero(this float value, float epsilon = DefaultFloatEpsilon)
        => Math.Abs(value) < epsilon;

    /// <summary>
    /// Maps a float value from one range to another
    /// </summary>
    public static float Map(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        if (Math.Abs(fromMax - fromMin) < float.Epsilon) return toMin;
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    /// <summary>
    /// Returns the percentage difference between two values
    /// </summary>
    public static float PercentageDifference(this float value1, float value2)
    {
        if (value1.IsEffectivelyZero() && value2.IsEffectivelyZero()) return 0;
        if (value1.IsEffectivelyZero() || value2.IsEffectivelyZero()) return 100;

        return Math.Abs((value1 - value2) / ((value1 + value2) / 2)) * 100;
    }

    /// <summary>
    /// Safe division that handles division by zero by returning a default value
    /// </summary>
    public static float SafeDivide(this float dividend, float divisor, float defaultValue = 0)
        => divisor.IsEffectivelyZero() ? defaultValue : dividend / divisor;
}