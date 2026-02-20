using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Utilities;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Http.TLS.Builders;

public class RequestBuilder
{
	private readonly Request _request = new();

	/// <summary>
	/// Sets request URL.
	/// </summary>
	public RequestBuilder WithUrl(string url)
	{
		_request.RequestUrl = url.ThrowIfNotUri();
		return this;
	}

	/// <summary>
	/// Sets HTTP method.
	/// </summary>
	public RequestBuilder WithMethod(HttpMethod method)
	{
		_request.RequestMethod = method.Method;
		return this;
	}

	/// <summary>
	/// Sets request timeout.
	/// </summary>
	public RequestBuilder WithTimeout(TimeSpan timeout)
	{
		_request.TimeoutMilliseconds = (int)timeout.ThrowIfInvalidTimeout().TotalMilliseconds;
		return this;
	}

	/// <summary>
	/// Adds single header.
	/// </summary>
	public RequestBuilder WithHeader(string name, string value)
	{
		_request.Headers[name.ThrowIfNullOrEmpty()] = value;
		return this;
	}

	/// <summary>
	/// Adds multiple headers.
	/// </summary>
	public RequestBuilder WithHeaders(Dictionary<string, string> values)
	{
		foreach (var kvp in values.ThrowIfNull())
		{
			WithHeader(kvp.Key, kvp.Value);
		}

		return this;
	}

	/// <summary>
	/// Sets custom header order (replaces existing).
	/// </summary>
	public RequestBuilder WithHeaderOrder(List<string> names)
	{
		_request.HeaderOrder = names.ThrowIfNull();
		return this;
	}

	/// <summary>
	/// Adds CONNECT headers for proxy tunneling.
	/// </summary>
	public RequestBuilder WithConnectHeaders(Dictionary<string, List<string>> values)
	{
		foreach (var kvp in values.ThrowIfNull())
		{
			_request.ConnectHeaders[kvp.Key.ThrowIfNullOrEmpty()] = kvp.Value;
		}

		return this;
	}

	/// <summary>
	/// Sets request body as string.
	/// </summary>
	public RequestBuilder WithBody(string? content)
	{
		_request.RequestBody = content;
		return this;
	}

	/// <summary>
	/// Serializes object to JSON and sets as body.
	/// </summary>
	public RequestBuilder WithBody<T>(T? obj) where T : class
	{
		try
		{
			WithBody(Serializer.Serialize(obj));
		}
		catch (JsonException ex)
		{
			throw new ArgumentException($"Failed to serialize data to JSON: {ex.Message}", nameof(obj), ex);
		}

		return WithHeader("Content-Type", "application/json");
	}

	/// <summary>
	/// Sets binary data as base64-encoded body.
	/// </summary>
	public RequestBuilder WithBody(byte[] bytes)
	{
		const int maxSize = 50 * 1024 * 1024;

		if (bytes.Length > maxSize)
		{
			throw new ArgumentException($"Binary data too large: {bytes.Length} bytes. Maximum allowed: {maxSize} bytes.", nameof(bytes));
		}

		WithBody(Convert.ToBase64String(bytes))
			.With(x => x.IsByteRequest = true);
		return this;
	}

	/// <summary>
	/// Adds single cookie.
	/// </summary>
	public RequestBuilder WithCookie(ClientCookie value)
	{
		_request.RequestCookies.Add(value);
		return this;
	}

	/// <summary>
	/// Adds cookies to request.
	/// </summary>
	public RequestBuilder WithCookies(List<ClientCookie> values)
	{
		_request.RequestCookies.AddRange(values.ThrowIfNull());
		return this;
	}

	/// <summary>
	/// Enables custom cookie jar.
	/// </summary>
	public RequestBuilder WithCookieJar(bool enable = true)
	{
		_request.WithCustomCookieJar = enable;
		return this;
	}

	/// <summary>
	/// Disables cookie jar.
	/// </summary>
	public RequestBuilder WithoutCookieJar(bool disable = true)
	{
		_request.WithoutCookieJar = disable;
		return this;
	}

	/// <summary>
	/// Configures proxy server.
	/// </summary>
	public RequestBuilder WithProxy(string url, bool rotating = false)
	{
		_request.ProxyUrl = url.ThrowIfNotUri();
		_request.IsRotatingProxy = rotating;
		return this;
	}

	/// <summary>
	/// Enables random TLS extension order.
	/// </summary>
	public RequestBuilder WithRandomTlsExtensions(bool enable = true)
	{
		_request.WithRandomTlsExtensionOrder = enable;
		return this;
	}

	/// <summary>
	/// Overrides SNI hostname.
	/// </summary>
	public RequestBuilder WithServerName(string serverName)
	{
		_request.ServerNameOverwrite = serverName.ThrowIfInvalidHostname();
		return this;
	}

	/// <summary>
	/// Overrides Host header.
	/// </summary>
	public RequestBuilder WithHostOverride(string hostOverride)
	{
		_request.RequestHostOverride = hostOverride.ThrowIfInvalidHostname();
		return this;
	}

	/// <summary>
	/// Skips SSL certificate verification.
	/// </summary>
	public RequestBuilder WithInsecureSkipVerify(bool skip = true)
	{
		_request.InsecureSkipVerify = skip;
		return this;
	}

	/// <summary>
	/// Configures certificate pinning.
	/// </summary>
	public RequestBuilder WithCertificatePinning(string hostname, List<string> fingerprints)
	{
		hostname.ThrowIfNullOrEmpty();
		fingerprints.ThrowIfNull().ThrowIfEmpty();

		_request.CertificatePinningHosts[hostname] = fingerprints;
		return this;
	}

	/// <summary>
	/// Configures transport options.
	/// </summary>
	public RequestBuilder WithTransportOptions(TransportOptions? options)
	{
		_request.TransportOptions = options;
		return this;
	}

	/// <summary>
	/// Sets local IP address for binding.
	/// </summary>
	public RequestBuilder WithLocalAddress(string address)
	{
		_request.LocalAddress = address.ThrowIfInvalidIpAddress();
		return this;
	}

	/// <summary>
	/// Disables HTTP/3 protocol.
	/// </summary>
	public RequestBuilder WithDisableHttp3(bool disable = true)
	{
		_request.DisableHttp3 = disable;
		return this;
	}

	/// <summary>
	/// Forces HTTP/1.1 protocol.
	/// </summary>
	public RequestBuilder WithForceHttp1(bool force = true)
	{
		_request.ForceHttp1 = force;
		return this;
	}

	/// <summary>
	/// Enables protocol racing.
	/// </summary>
	public RequestBuilder WithProtocolRacing(bool enable = true)
	{
		_request.WithProtocolRacing = enable;
		return this;
	}

	/// <summary>
	/// Disables IPv4.
	/// </summary>
	public RequestBuilder WithDisableIPv4(bool disable = true)
	{
		_request.DisableIPv4 = disable;
		return this;
	}

	/// <summary>
	/// Disables IPv6.
	/// </summary>
	public RequestBuilder WithDisableIPv6(bool disable = true)
	{
		_request.DisableIPv6 = disable;
		return this;
	}

	/// <summary>
	/// Enables binary response mode.
	/// </summary>
	public RequestBuilder WithByteResponse(bool enable = true)
	{
		_request.IsByteResponse = enable;
		return this;
	}

	/// <summary>
	/// Enables EUC-KR response encoding.
	/// </summary>
	public RequestBuilder WithEuckrResponse(bool enable = true)
	{
		_request.EuckrResponse = enable;
		return this;
	}

	/// <summary>
	/// Enables automatic redirect following.
	/// </summary>
	public RequestBuilder WithFollowRedirects(bool follow = true)
	{
		_request.FollowRedirects = follow;
		return this;
	}

	/// <summary>
	/// Configures stream output to file.
	/// </summary>
	public RequestBuilder WithStreamOutput(string? filePath, int? size = null, string? eof = null)
	{
		if (size is <= 0)
		{
			throw new ArgumentException("StreamOutputBlockSize must be positive if specified.", nameof(size));
		}

		_request.StreamOutputPath = filePath;
		_request.StreamOutputBlockSize = size;
		_request.StreamOutputEofSymbol = eof;
		return this;
	}

	/// <summary>
	/// Enables debug mode.
	/// </summary>
	public RequestBuilder WithDebug(bool enable = true)
	{
		_request.WithDebug = enable;
		return this;
	}

	/// <summary>
	/// Enables panic recovery.
	/// </summary>
	public RequestBuilder WithCatchPanics(bool enable = true)
	{
		_request.CatchPanics = enable;
		return this;
	}

	/// <summary>
	/// Applies custom configuration.
	/// </summary>
	public RequestBuilder With(Action<Request>? config)
	{
		config?.Invoke(_request);
		return this;
	}

	/// <summary>
	/// Builds the request.
	/// </summary>
	public Request Build()
	{
		return _request;
	}
}