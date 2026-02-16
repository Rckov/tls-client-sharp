using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;

namespace Http.TLS.Examples.Examples;

public sealed class BasicExample : IExample
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
			.Build();
	}

	private static Request CreateRequest()
	{
		return new Request
		{
			RequestUrl = "https://httpbin.org/get",
			RequestMethod = "GET"
		};
	}

	private static void PrintResponse(Response? response)
	{
		if (response == null)
		{
			return;
		}

		Console.WriteLine($"Status: {response.Status}");
		Console.WriteLine($"Body: {response.Body}");
	}
}