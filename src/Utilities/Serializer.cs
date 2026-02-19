using Http.TLS.Core.Converters;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Http.TLS.Utilities;

public static class Serializer
{
	private static readonly JsonSerializerOptions Options = new()
	{
		WriteIndented = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Converters =
		{
			new BrowserTypeConverter()
		}
	};

	public static string Serialize<T>(T? data) where T : class
	{
		data.ThrowIfNull();
		return JsonSerializer.Serialize(data, Options);
	}

	public static byte[] SerializeToBytes<T>(T? data) where T : class
	{
		var value = Serialize(data);
		return Encoding.UTF8.GetBytes(value);
	}

	public static T? Deserialize<T>(string? json) where T : class
	{
		json.ThrowIfNullOrEmpty();
		return JsonSerializer.Deserialize<T>(json!, Options);
	}
}