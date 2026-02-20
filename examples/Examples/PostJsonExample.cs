using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;
using Http.TLS.Extensions;

using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Examples;

public sealed class PostJsonExample(IRequestClientFactory factory) : IExample
{
	public async Task RunAsync()
	{
		using var client = factory.CreateClient();

		var payload = new PostPayload("[name]", "[email]", 30);

		var response = await client.PostJsonAsync("https://httpbin.org/post", payload);
		PrintResponse(response);
	}

	private static void PrintResponse(Response? response)
	{
		if (response is null)
		{
			return;
		}

		Console.WriteLine($"POST Response: {response.Body}");
	}

	public sealed class PostPayload(string name, string email, int age)
	{
		public string Name { get; } = name;
		public string Email { get; } = email;
		public int Age { get; } = age;
	}
}

[JsonSerializable(typeof(PostJsonExample.PostPayload))]
public partial class ExamplesJsonContext : JsonSerializerContext
{
}