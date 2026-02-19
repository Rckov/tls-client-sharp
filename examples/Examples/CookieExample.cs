using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Examples;

public sealed class CookieExample(IRequestClientFactory factory) : IExample
{
	public async Task RunAsync()
	{
		using var client = factory.CreateClient(o => o.WithCustomCookieJar = true);

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/cookies")
			.WithMethod(HttpMethod.Get)
			.WithCookie(new ClientCookie("session", "abc123"))
			.WithCookie(new ClientCookie("user_id", "12345"))
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

		Console.WriteLine($"Response: {response.Body}");
	}
}