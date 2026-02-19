using Http.TLS.Builders;

using System;
using System.Collections.Generic;

namespace Http.TLS;

public class RequestClientFactory(Action<RequestClientOptions>? defaultConfigure = null) : IRequestClientFactory
{
	private readonly Dictionary<string, Action<RequestClientOptions>> _named = new(StringComparer.Ordinal);

	/// <summary>
	/// Registers a named configuration.
	/// </summary>
	public RequestClientFactory Register(string name, Action<RequestClientOptions> configure)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));
		}

		_named[name] = configure ?? throw new ArgumentNullException(nameof(configure));
		return this;
	}

	/// <inheritdoc />
	public IRequestClient CreateClient() => Build(defaultConfigure);

	/// <inheritdoc />
	public IRequestClient CreateClient(string name)
	{
		if (!_named.TryGetValue(name, out var configure))
		{
			throw new KeyNotFoundException($"No configuration registered for name '{name}'.");
		}

		return Build(configure);
	}

	/// <inheritdoc />
	public IRequestClient CreateClient(Action<RequestClientOptions> configure) => Build(configure);

	private static IRequestClient Build(Action<RequestClientOptions>? configure)
	{
		var builder = new RequestClientBuilder();
		configure?.Invoke(builder.Options);
		return builder.Build();
	}
}