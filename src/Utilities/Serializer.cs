using Http.TLS.Core.Converters;
using Http.TLS.Native;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Http.TLS.Utilities;

public static class Serializer
{
	private static JsonSerializerOptions? _options;
	private static JsonSerializerOptions Options => _options ??= CreateOptions();

	private static JsonSerializerOptions CreateOptions()
	{
#if NET8_0_OR_GREATER
		var options = new JsonSerializerOptions(SerializerContext.Default.Options);
		foreach (var context in NativeClientContext.RegisteredContexts)
		{
			options.TypeInfoResolverChain.Add(context);
		}
#else
		var options = new JsonSerializerOptions();
#endif
		options.WriteIndented = false;
		options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
		options.Converters.Add(new BrowserTypeConverter());
		return options;
	}

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