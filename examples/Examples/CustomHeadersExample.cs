using Http.TLS.Builders;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Examples;

public sealed class CustomHeadersExample(IRequestClientFactory factory) : IExample
{
	public async Task RunAsync()
	{
		using var client = factory.CreateClient(o => o.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/headers")
			.WithMethod(HttpMethod.Get)
			.WithHeader("X-Custom-Header", "CustomValue")
			.WithHeader("X-API-Key", "secret-key")
			.WithHeaderOrder(["X-Custom-Header", "X-API-Key", "User-Agent"])
			.Build();

		var response = await client.SendAsync(request);
		PrintResponse(response);
	}

	private static void PrintResponse(Response? response)
	{
		if (response is null)
		{
			return;
		}

		Console.WriteLine($"Headers Response: {response.Body}");
	}
}