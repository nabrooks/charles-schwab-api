using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.V2.Shared.Utilities
{
    internal static class JsonConfig
    {
        private static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            FloatParseHandling = FloatParseHandling.Decimal,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Error = delegate(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
            {
                if (args.CurrentObject == args.ErrorContext.OriginalObject)
                {
                    Console.WriteLine("Json serialization error {@OriginalObject} {@Member} {@ErrorMessage}"
                                                            , args.ErrorContext.OriginalObject
                                                            , args.ErrorContext.Member
                                                            , args.ErrorContext.Error.Message);
                }
            }
        };

        internal static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
        }

        internal static T? DeserializeObject<T>(string contentBody)
        {
            return JsonConvert.DeserializeObject<T>(contentBody, SerializerSettings);
        }
    }
}
