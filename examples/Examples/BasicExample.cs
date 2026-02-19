using Http.TLS.Core.Response;
using Http.TLS.Examples.Abstractions;
using Http.TLS.Extensions;

using System;
using System.Threading.Tasks;

namespace Http.TLS.Examples.Examples;

public sealed class BasicExample(IRequestClientFactory factory) : IExample
{
	public async Task RunAsync()
	{
		using var client = factory.CreateClient();

		var response = await client.GetAsync("https://httpbin.org/get");
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