using Http.TLS.Examples.Abstractions;
using Http.TLS.Examples.Examples;
using Http.TLS.Native;

using System;

namespace Http.TLS.Examples;

public static class Program
{
	private static void Main()
	{
		using var context = new NativeClientContext("tls-client-windows-64-1.14.0.dll");

		var examples = new IExample[]
		{
			new BasicExample(),
			new CookieExample(),
			new CustomHeadersExample(),
			new PostJsonExample(),
			new CustomRequestClientExample()
		};

		for (int i = 0; i < examples.Length; i++)
		{
			Console.WriteLine($"{i + 1}. {examples[i].GetType().Name}");
			examples[i].Run();
		}

		// context?.Dispose();
	}
}