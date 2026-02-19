using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Converters;

/// <summary>
/// JSON converter for nullable booleans that serializes null as false.
/// </summary>
public class NullableBoolToFalseConverter : JsonConverter<bool?>
{
	public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return null;
		}

		if (reader.TokenType == JsonTokenType.True)
		{
			return true;
		}

		if (reader.TokenType == JsonTokenType.False)
		{
			return false;
		}

		throw new JsonException($"Cannot convert {reader.TokenType} to bool?");
	}

	public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
	{
		writer.WriteBooleanValue(value ?? false);
	}
}