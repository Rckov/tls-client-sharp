using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;
using System.Net.Http;

namespace Http.TLS.Examples.Examples;

public sealed class CookieExample : IExample
{
	public void Run()
	{
		using var client = CreateClient();

		var request = CreateRequest();
		var response = client.Send(request);

		PrintResponse(response);
	}

	private static IRequestClient CreateClient()
	{
		return new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.WithCookieJar(true)
			.Build();
	}

	private static Request CreateRequest()
	{
		return new RequestBuilder()
			.WithUrl("https://httpbin.org/cookies")
			.WithMethod(HttpMethod.Get)
			.WithCookie(new ClientCookie("session", "abc123"))
			.WithCookie(new ClientCookie("user_id", "12345"))
			.Build();
	}

	private static void PrintResponse(Response? response)
	{
		if (response == null)
		{
			return;
		}

		Console.WriteLine($"Response: {response.Body}");
	}
}