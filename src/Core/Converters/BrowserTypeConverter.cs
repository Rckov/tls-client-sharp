using Http.TLS.Extensions;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Converters;

/// <summary>
/// JSON converter for <see cref="BrowserType" /> enum.
/// </summary>
internal class BrowserTypeConverter : JsonConverter<BrowserType>
{
	/// <summary>
	/// Reads <see cref="BrowserType" /> from JSON.
	/// </summary>
	public override BrowserType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType switch
		{
			JsonTokenType.Null => default,
			JsonTokenType.String => string.IsNullOrEmpty(reader.GetString())
				? default
				: BrowserTypeExtensions.Parse(reader.GetString()),

			_ => throw new JsonException($"Cannot convert {reader.TokenType} to BrowserType")
		};
	}

	/// <summary>
	/// Writes <see cref="BrowserType" /> to JSON.
	/// </summary>
	public override void Write(Utf8JsonWriter writer, BrowserType value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToDescription());
	}
}