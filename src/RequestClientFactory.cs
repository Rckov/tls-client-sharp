using Http.TLS.Builders;
using Http.TLS.Utilities;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Http.TLS;

public class RequestClientFactory(Action<RequestClientOptions>? defaultConfigure = null) : IRequestClientFactory
{
	private readonly ConcurrentDictionary<string, Action<RequestClientOptions>> _named = new(StringComparer.Ordinal);

	/// <inheritdoc />
	public IRequestClientFactory Register(string name, Action<RequestClientOptions> configure)
	{
		_named[name.ThrowIfNullOrEmpty()] = configure.ThrowIfNull();
		return this;
	}

	/// <inheritdoc />
	public IRequestClient CreateClient() => Build(null);

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

	private IRequestClient Build(Action<RequestClientOptions>? configure)
	{
		var builder = new RequestClientBuilder();
		defaultConfigure?.Invoke(builder.Options);
		configure?.Invoke(builder.Options);
		return builder.Build();
	}
}