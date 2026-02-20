using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;
using Http.TLS.Extensions;

using System;
using System.Collections.Generic;
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

		Console.WriteLine("Session example:");
		await SessionExample(cookieContainer);
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

	private async Task SessionExample(CookieContainer cookies)
	{
		using var client = factory.CreateClient(o => o.WithCustomCookieJar = true);

		var requestBuilder = new RequestBuilder()
			.WithUrl($"{URL}/cookies")
			.WithMethod(HttpMethod.Get)
			.Build();

		var cookiesList = new List<ClientCookie>();
		foreach (Cookie cookie in cookies.GetCookies(new Uri(URL)))
		{
			cookiesList.Add(cookie.ToClientCookie());
		}

		var response = await client.SendAsync(requestBuilder);

		client.AddCookies(URL, cookiesList);
		requestBuilder = new RequestBuilder()
			.WithUrl($"{URL}/cookies")
			.WithMethod(HttpMethod.Get)
			.Build();

		response = await client.SendAsync(requestBuilder);
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