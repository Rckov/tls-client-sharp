using Http.TLS.Builders;
using Http.TLS.Utilities;

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
		_named[name.ThrowIfNullOrEmpty()] = configure.ThrowIfNull();
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