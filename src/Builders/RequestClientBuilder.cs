using Http.TLS.Core;
using Http.TLS.Utilities;

using System;
using System.Collections.Generic;

namespace Http.TLS.Builders;

/// <summary>
/// Builder for client configuration options.
/// </summary>
public class RequestClientBuilder
{
	private readonly RequestClientOptions _options = new();

	/// <summary>
	/// Sets request timeout.
	/// </summary>
	public RequestClientBuilder WithTimeout(TimeSpan timeout)
	{
		_options.Timeout = timeout.ThrowIfInvalidTimeout(nameof(timeout));
		return this;
	}

	/// <summary>
	/// Sets custom TLS client.
	/// </summary>
	public RequestClientBuilder WithCustomRequestClient(CustomRequestClient? client)
	{
		_options.CustomRequestClient = client.ThrowIfNull(nameof(client));
		return this;
	}

	/// <summary>
	/// Sets browser fingerprint type.
	/// </summary>
	public RequestClientBuilder WithBrowserType(BrowserType type)
	{
		_options.BrowserType = type;
		return this;
	}

	/// <summary>
	/// Configures transport options.
	/// </summary>
	public RequestClientBuilder WithTransportOptions(TransportOptions? options)
	{
		_options.TransportOptions = options.ThrowIfNull(nameof(options));
		return this;
	}

	/// <summary>
	/// Sets default headers (replaces existing).
	/// </summary>
	public RequestClientBuilder WithDefaultHeaders(Dictionary<string, List<string>> headers)
	{
		_options.DefaultHeaders = headers.ThrowIfNull(nameof(headers));
		return this;
	}

	/// <summary>
	/// Sets custom header order (replaces existing).
	/// </summary>
	public RequestClientBuilder WithHeaderOrder(List<string> names)
	{
		_options.HeaderOrder = names.ThrowIfNull(nameof(names));
		return this;
	}

	/// <summary>
	/// Sets User-Agent header.
	/// </summary>
	public RequestClientBuilder WithUserAgent(string? agent)
	{
		_options.UserAgent = agent;
		return this;
	}

	/// <summary>
	/// Sets proxy URL.
	/// </summary>
	public RequestClientBuilder WithProxy(string url, bool rotating = false)
	{
		_options.ProxyUrl = url.ThrowIfNullOrEmpty(nameof(url));
		_options.IsRotatingProxy = rotating;
		return this;
	}

	/// <summary>
	/// Sets local IP address for binding.
	/// </summary>
	public RequestClientBuilder WithLocalAddress(string address)
	{
		_options.LocalAddress = address.ThrowIfInvalidIpAddress(nameof(address));
		return this;
	}

	/// <summary>
	/// Disables HTTP/3 protocol.
	/// </summary>
	public RequestClientBuilder WithDisableHttp3(bool disable = true)
	{
		_options.DisableHttp3 = disable;
		return this;
	}

	/// <summary>
	/// Disables IPv4.
	/// </summary>
	public RequestClientBuilder WithDisableIPv4(bool disable = true)
	{
		_options.DisableIPv4 = disable;
		return this;
	}

	/// <summary>
	/// Disables IPv6.
	/// </summary>
	public RequestClientBuilder WithDisableIPv6(bool disable = true)
	{
		_options.DisableIPv6 = disable;
		return this;
	}

	/// <summary>
	/// Enables protocol racing.
	/// </summary>
	public RequestClientBuilder WithProtocolRacing(bool enable = true)
	{
		_options.WithProtocolRacing = enable;
		return this;
	}

	/// <summary>
	/// Forces HTTP/1.1 protocol.
	/// </summary>
	public RequestClientBuilder WithForceHttp1(bool force = true)
	{
		_options.ForceHttp1 = force;
		return this;
	}

	/// <summary>
	/// Skips SSL certificate verification.
	/// </summary>
	public RequestClientBuilder WithInsecureSkipVerify(bool skip = true)
	{
		_options.InsecureSkipVerify = skip;
		return this;
	}

	/// <summary>
	/// Enables random TLS extension order.
	/// </summary>
	public RequestClientBuilder WithRandomTlsExtensions(bool enable = true)
	{
		_options.WithRandomTlsExtensionOrder = enable;
		return this;
	}

	/// <summary>
	/// Sets certificate pinning.
	/// </summary>
	public RequestClientBuilder WithCertificatePinning(string host, List<string> fingerprints)
	{
		host.ThrowIfNullOrEmpty(nameof(host));
		fingerprints.ThrowIfNull(nameof(fingerprints)).ThrowIfEmpty(nameof(fingerprints));

		_options.CertificatePinningHosts[host] = fingerprints;
		return this;
	}

	/// <summary>
	/// Enables custom cookie jar.
	/// </summary>
	public RequestClientBuilder WithCookieJar(bool enable = true)
	{
		_options.WithCustomCookieJar = enable;
		return this;
	}

	/// <summary>
	/// Disables cookie jar.
	/// </summary>
	public RequestClientBuilder WithoutCookieJar(bool disable = true)
	{
		_options.WithoutCookieJar = disable;
		return this;
	}

	/// <summary>
	/// Enables debug mode.
	/// </summary>
	public RequestClientBuilder WithDebug(bool enable = true)
	{
		_options.WithDebug = enable;
		return this;
	}

	/// <summary>
	/// Enables panic recovery.
	/// </summary>
	public RequestClientBuilder WithCatchPanics(bool enable = true)
	{
		_options.CatchPanics = enable;
		return this;
	}

	/// <summary>
	/// Applies custom configuration.
	/// </summary>
	public RequestClientBuilder With(Action<RequestClientOptions> config)
	{
		config?.Invoke(_options);
		return this;
	}

	/// <summary>
	/// Builds the client.
	/// </summary>
	public RequestClient Build()
	{
		_options.Validate();
		return new RequestClient(_options.Clone());
	}
}