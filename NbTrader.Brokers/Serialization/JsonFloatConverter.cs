using System.Text.Json;
using System.Text.Json.Serialization;

namespace NbTrader.Utility.Serialization
{
    public class JsonFloatConverter : JsonConverter<float>
	{
		public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String && reader.GetString() == "NaN")
				return float.NaN;

			return reader.GetSingle(); // JsonException thrown if reader.TokenType != JsonTokenType.Number
		}

		public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
		{
			if (float.IsNaN(value))
				writer.WriteStringValue("NaN");
			else
				writer.WriteNumberValue(value);
		}
	}
}
