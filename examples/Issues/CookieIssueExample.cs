using Http.TLS.Builders;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;
using Http.TLS.Extensions;

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Issues;

public sealed class CookieIssueExample(IRequestClientFactory factory) : IExample
{
	private const string URL = "https://httpbin.org";

	public async Task RunAsync()
	{
		var cookieContainer = new CookieContainer();
		cookieContainer.Add(new Uri(URL), new Cookie("session_id", "abc123"));
		cookieContainer.Add(new Uri(URL), new Cookie("user_token", "xyz789"));
		cookieContainer.Add(new Uri(URL), new Cookie("preferences", "dark_mode"));

		Console.WriteLine("User example:");
		await UserExample(cookieContainer);

		Console.WriteLine("JAR example:");
		await JARExample(cookieContainer);
	}

	private async Task UserExample(CookieContainer cookies)
	{
		using var client = factory.CreateClient();
		var requestBuilder = new RequestBuilder()
			.WithUrl($"https://httpbin.org/cookies")
			.WithCookieJar()
			.WithMethod(HttpMethod.Get);

		foreach (Cookie cookie in cookies.GetCookies(new Uri(URL)))
		{
			requestBuilder.WithHeader("Cookie", $"{cookie.Name}={cookie.Value}");
		}

		var response = await client.SendAsync(requestBuilder.Build());
		PrintResponse(response);
	}

	private async Task JARExample(CookieContainer cookies)
	{
		using var client = factory.CreateClient();
		var requestBuilder = new RequestBuilder()
			.WithUrl($"https://httpbin.org/cookies")
			.WithCookieJar()
			.WithMethod(HttpMethod.Get);

		foreach (Cookie cookie in cookies.GetCookies(new Uri(URL)))
		{
			requestBuilder.WithCookie(cookie.ToClientCookie());
		}

		var response = await client.SendAsync(requestBuilder.Build());
		PrintResponse(response);
	}

	private static void PrintResponse(Response? response)
	{
		if (response is null)
		{
			return;
		}

		Console.WriteLine($"Status: {response.Status}");
		Console.WriteLine($"Body: {response.Body}");
	}
}
