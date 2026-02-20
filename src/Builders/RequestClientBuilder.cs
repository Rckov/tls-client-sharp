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
	/// <summary>
	/// Direct access to the underlying options for factory scenarios.
	/// </summary>
	public RequestClientOptions Options { get; } = new();

	/// <summary>
	/// Sets request timeout.
	/// </summary>
	public RequestClientBuilder WithTimeout(TimeSpan timeout)
	{
		Options.Timeout = timeout.ThrowIfInvalidTimeout();
		return this;
	}

	/// <summary>
	/// Sets custom TLS client.
	/// </summary>
	public RequestClientBuilder WithCustomRequestClient(CustomRequestClient? client)
	{
		Options.CustomRequestClient = client.ThrowIfNull();
		return this;
	}

	/// <summary>
	/// Sets browser fingerprint type.
	/// </summary>
	public RequestClientBuilder WithBrowserType(BrowserType type)
	{
		Options.BrowserType = type;
		return this;
	}

	/// <summary>
	/// Configures transport options.
	/// </summary>
	public RequestClientBuilder WithTransportOptions(TransportOptions? options)
	{
		Options.TransportOptions = options.ThrowIfNull();
		return this;
	}

	/// <summary>
	/// Sets default headers (replaces existing).
	/// </summary>
	public RequestClientBuilder WithDefaultHeaders(Dictionary<string, List<string>> headers)
	{
		Options.DefaultHeaders = headers.ThrowIfNull();
		return this;
	}

	/// <summary>
	/// Sets custom header order (replaces existing).
	/// </summary>
	public RequestClientBuilder WithHeaderOrder(List<string> names)
	{
		Options.HeaderOrder = names.ThrowIfNull();
		return this;
	}

	/// <summary>
	/// Sets User-Agent header.
	/// </summary>
	public RequestClientBuilder WithUserAgent(string? agent)
	{
		Options.UserAgent = agent;
		return this;
	}

	/// <summary>
	/// Sets proxy URL.
	/// </summary>
	public RequestClientBuilder WithProxy(string url, bool rotating = false)
	{
		Options.ProxyUrl = url.ThrowIfNotUri();
		Options.IsRotatingProxy = rotating;
		return this;
	}

	/// <summary>
	/// Sets local IP address for binding.
	/// </summary>
	public RequestClientBuilder WithLocalAddress(string address)
	{
		Options.LocalAddress = address.ThrowIfInvalidIpAddress();
		return this;
	}

	/// <summary>
	/// Disables HTTP/3 protocol.
	/// </summary>
	public RequestClientBuilder WithDisableHttp3(bool disable = true)
	{
		Options.DisableHttp3 = disable;
		return this;
	}

	/// <summary>
	/// Disables IPv4.
	/// </summary>
	public RequestClientBuilder WithDisableIPv4(bool disable = true)
	{
		Options.DisableIPv4 = disable;
		return this;
	}

	/// <summary>
	/// Disables IPv6.
	/// </summary>
	public RequestClientBuilder WithDisableIPv6(bool disable = true)
	{
		Options.DisableIPv6 = disable;
		return this;
	}

	/// <summary>
	/// Enables protocol racing.
	/// </summary>
	public RequestClientBuilder WithProtocolRacing(bool enable = true)
	{
		Options.WithProtocolRacing = enable;
		return this;
	}

	/// <summary>
	/// Forces HTTP/1.1 protocol.
	/// </summary>
	public RequestClientBuilder WithForceHttp1(bool force = true)
	{
		Options.ForceHttp1 = force;
		return this;
	}

	/// <summary>
	/// Skips SSL certificate verification.
	/// </summary>
	public RequestClientBuilder WithInsecureSkipVerify(bool skip = true)
	{
		Options.InsecureSkipVerify = skip;
		return this;
	}

	/// <summary>
	/// Enables random TLS extension order.
	/// </summary>
	public RequestClientBuilder WithRandomTlsExtensions(bool enable = true)
	{
		Options.WithRandomTlsExtensionOrder = enable;
		return this;
	}

	/// <summary>
	/// Sets certificate pinning.
	/// </summary>
	public RequestClientBuilder WithCertificatePinning(string host, List<string> fingerprints)
	{
		host.ThrowIfNullOrEmpty();
		fingerprints.ThrowIfNull().ThrowIfEmpty();

		Options.CertificatePinningHosts[host] = fingerprints;
		return this;
	}

	/// <summary>
	/// Enables custom cookie jar.
	/// </summary>
	public RequestClientBuilder WithCookieJar(bool enable = true)
	{
		Options.WithCustomCookieJar = enable;
		return this;
	}

	/// <summary>
	/// Disables cookie jar.
	/// </summary>
	public RequestClientBuilder WithoutCookieJar(bool disable = true)
	{
		Options.WithoutCookieJar = disable;
		return this;
	}

	/// <summary>
	/// Enables debug mode.
	/// </summary>
	public RequestClientBuilder WithDebug(bool enable = true)
	{
		Options.WithDebug = enable;
		return this;
	}

	/// <summary>
	/// Enables panic recovery.
	/// </summary>
	public RequestClientBuilder WithCatchPanics(bool enable = true)
	{
		Options.CatchPanics = enable;
		return this;
	}

	/// <summary>
	/// Applies custom configuration.
	/// </summary>
	public RequestClientBuilder With(Action<RequestClientOptions> config)
	{
		config?.Invoke(Options);
		return this;
	}

	/// <summary>
	/// Builds the client.
	/// </summary>
	public RequestClient Build()
	{
		Options.Validate();
		return new RequestClient(Options.Clone());
	}
}