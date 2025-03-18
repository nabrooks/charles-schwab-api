using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetEnumMemberValue<T>(this T value)
            where T : struct, IConvertible
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString(CultureInfo.InvariantCulture))
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }
    }
}
