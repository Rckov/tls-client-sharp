using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;
using Http.TLS.Extensions;

using System;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Examples;

public sealed class PostJsonExample(IRequestClientFactory factory) : IExample
{
	private sealed record PostPayload(string Name, string Email, int Age);

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
}