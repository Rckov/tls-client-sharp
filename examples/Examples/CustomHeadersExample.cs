using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;
using System.Net.Http;

namespace Http.TLS.Examples.Examples;

public sealed class CustomHeadersExample : IExample
{
	public void Run()
	{
		using var client = CreateClient();
		var request = CreateRequestWithHeaders();
		var response = client.Send(request);

		PrintResponse(response);
	}

	private static IRequestClient CreateClient()
	{
		return new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64)")
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();
	}

	private static Request CreateRequestWithHeaders()
	{
		return new RequestBuilder()
			.WithUrl("https://httpbin.org/headers")
			.WithMethod(HttpMethod.Get)
			.WithHeader("X-Custom-Header", "CustomValue")
			.WithHeader("X-API-Key", "secret-key")
			.WithHeaderOrder(
			[
				"X-Custom-Header",
				"X-API-Key",
				"User-Agent"
			])
			.Build();
	}

	private static void PrintResponse(Response? response)
	{
		if (response == null)
		{
			return;
		}

		Console.WriteLine($"Headers Response: {response.Body}");
	}
}