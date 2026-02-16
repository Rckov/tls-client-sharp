using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;

using System;
using System.Net.Http;

namespace Http.TLS.Examples.Examples;

public sealed class PostJsonExample : IExample
{
	public void Run()
	{
		using var client = CreateClient();
		var request = CreatePostRequest();
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

	private static Request CreatePostRequest()
	{
		var data = new
		{
			name = "John Doe",
			email = "john@example.com",
			age = 30
		};

		return new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(data)
			.Build();
	}

	private static void PrintResponse(Response? response)
	{
		if (response == null)
		{
			return;
		}

		Console.WriteLine($"POST Response: {response.Body}");
	}
}