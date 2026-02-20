using Http.TLS.Examples.Abstractions;
using Http.TLS.Examples.Examples;
using Http.TLS.Examples.Issues;
using Http.TLS.Native;

using System;
using System.Threading.Tasks;

namespace Http.TLS.Examples;

public static class Program
{
	private static async Task Main()
	{
		using var context = new NativeClientContext("tls-client-windows-64-1.14.0.dll");
#if NET8_0_OR_GREATER
		context.RegisterContext(ExamplesJsonContext.Default);
#endif

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

		IExample[] Issues =
		[
			new CookieIssueExample(factory),
		];

		IExample[] array = examples;

		for (var i = 0; i < array.Length; i++)
		{
			Console.WriteLine($"{i + 1}. {array[i].GetType().Name}");
			await array[i].RunAsync();
		}

		Console.ReadLine();
	}
}