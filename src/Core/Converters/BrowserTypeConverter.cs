using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Converters;

internal sealed class BrowserTypeConverter : JsonConverter<BrowserType?>
{
	public override BrowserType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return null;
		}

		var value = reader.GetString();
		return string.IsNullOrEmpty(value) ? null : new BrowserType(value!);
	}

	public override void Write(Utf8JsonWriter writer, BrowserType? value, JsonSerializerOptions options)
	{
		if (value is null)
		{
			writer.WriteNullValue();
		}
		else
		{
			writer.WriteStringValue(value.Value);
		}
	}
}