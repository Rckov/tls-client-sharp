using Http.TLS.Examples.Abstractions;
using Http.TLS.Examples.Examples;
using Http.TLS.Native;

using System;
using System.Threading.Tasks;

namespace Http.TLS.Examples;

public static class Program
{
	private static async Task Main()
	{
		using var context = new NativeClientContext("tls-client-windows-64-1.14.0.dll");

		var factory = new RequestClientFactory();
		factory.Register("custom-tls", o => o.CustomRequestClient = CustomRequestClientExample.BuildProfile());

		IExample[] examples =
		[
			new BasicExample(factory),
			new CookieExample(factory),
			new CustomHeadersExample(factory),
			new PostJsonExample(factory),
			new CustomRequestClientExample(factory),
		];

		for (var i = 0; i < examples.Length; i++)
		{
			Console.WriteLine($"{i + 1}. {examples[i].GetType().Name}");
			await examples[i].RunAsync();
		}
	}
}