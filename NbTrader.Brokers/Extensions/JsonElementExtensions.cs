using System.Text.Json;

namespace NbTrader.Brokers.Extensions
{
    public static class JsonElementExtensions
    {
        public static T Value<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            T elementValue = options == null ? 
                JsonSerializer.Deserialize<T>(element) : 
                JsonSerializer.Deserialize<T>(element, options);

            if (elementValue == null)
                throw new ArgumentNullException("JsonElement deserialiation value is null.");

            return elementValue;
        }
    }
}
