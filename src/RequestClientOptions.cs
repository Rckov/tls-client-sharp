using Http.TLS.Core;
using Http.TLS.Utilities;

using System;
using System.Collections.Generic;
using System.Net;

namespace Http.TLS;

/// <summary>
/// Client configuration options.
/// </summary>
public sealed class RequestClientOptions
{
	/// <summary>
	/// Session identifier.
	/// </summary>
	public Guid SessionId { get; set; } = Guid.NewGuid();

	/// <summary>
	/// Request timeout.
	/// </summary>
	public TimeSpan Timeout { get; set; }

	/// <summary>
	/// Default headers.
	/// </summary>
	public Dictionary<string, List<string>> DefaultHeaders { get; set; } = [];

	/// <summary>
	/// Header order.
	/// </summary>
	public List<string> HeaderOrder { get; set; } = [];

	/// <summary>
	/// Custom cookie jar.
	/// </summary>
	public bool WithCustomCookieJar { get; set; }

	/// <summary>
	/// Disables cookie handling.
	/// </summary>
	public bool WithoutCookieJar { get; set; }

	/// <summary>
	/// Proxy URL.
	/// </summary>
	public string? ProxyUrl { get; set; }

	/// <summary>
	/// Rotating proxy flag.
	/// </summary>
	public bool IsRotatingProxy { get; set; }

	/// <summary>
	/// User-Agent header.
	/// </summary>
	public string? UserAgent
	{
		get => DefaultHeaders.TryGetValue("User-Agent", out var values) && values.Count > 0 ? values[0] : null;
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				DefaultHeaders.Remove("User-Agent");
			}
			else
			{
				DefaultHeaders["User-Agent"] = [value!];
			}
		}
	}

	/// <summary>
	/// Browser fingerprint.
	/// </summary>
	public BrowserType? BrowserType { get; set; }

	/// <summary>
	/// Skips SSL verification.
	/// </summary>
	public bool InsecureSkipVerify { get; set; }

	/// <summary>
	/// Randomizes TLS extensions.
	/// </summary>
	public bool WithRandomTlsExtensionOrder { get; set; }

	/// <summary>
	/// Certificate pinning.
	/// </summary>
	public Dictionary<string, List<string>> CertificatePinningHosts { get; set; } = [];

	/// <summary>
	/// Disables IPv4.
	/// </summary>
	public bool DisableIPv4 { get; set; }

	/// <summary>
	/// Disables IPv6.
	/// </summary>
	public bool DisableIPv6 { get; set; }

	/// <summary>
	/// Local IP address for binding.
	/// </summary>
	public string? LocalAddress { get; set; }

	/// <summary>
	/// Forces HTTP/1.1.
	/// </summary>
	public bool ForceHttp1 { get; set; }

	/// <summary>
	/// Disables HTTP/3.
	/// </summary>
	public bool DisableHttp3 { get; set; }

	/// <summary>
	/// Enables protocol racing.
	/// </summary>
	public bool WithProtocolRacing { get; set; }

	/// <summary>
	/// Debug mode.
	/// </summary>
	public bool WithDebug { get; set; }

	/// <summary>
	/// Catches panics.
	/// </summary>
	public bool CatchPanics { get; set; } = true;

	/// <summary>
	/// Custom TLS client.
	/// </summary>
	public CustomRequestClient? CustomRequestClient { get; set; }

	/// <summary>
	/// Transport options.
	/// </summary>
	public TransportOptions? TransportOptions { get; set; }

	/// <summary>
	/// Validates configuration.
	/// </summary>
	public void Validate()
	{
		if (SessionId == Guid.Empty)
		{
			throw new ArgumentException("SessionId must be non-empty.", nameof(SessionId));
		}

		if (Timeout <= TimeSpan.Zero)
		{
			throw new ArgumentException("Timeout must be positive.", nameof(Timeout));
		}

		if (!string.IsNullOrWhiteSpace(ProxyUrl))
		{
			ProxyUrl.IsUri();
		}

		if (!string.IsNullOrEmpty(LocalAddress) && !IPAddress.TryParse(LocalAddress, out _))
		{
			throw new ArgumentException("Local address must be a valid IP address.", nameof(LocalAddress));
		}

		foreach (var host in CertificatePinningHosts)
		{
			if (Uri.CheckHostName(host.Key) == UriHostNameType.Unknown)
			{
				throw new ArgumentException($"Invalid certificate pinning host: '{host.Key}'.", nameof(CertificatePinningHosts));
			}

			host.Value.ThrowIfEmpty(nameof(CertificatePinningHosts));
		}

		if (WithCustomCookieJar && WithoutCookieJar)
		{
			throw new InvalidOperationException("Cannot enable both WithCustomCookieJar and WithoutCookieJar.");
		}

		if (DisableIPv4 && DisableIPv6)
		{
			throw new InvalidOperationException("Cannot disable both IPv4 and IPv6.");
		}
	}

	/// <summary>
	/// Creates a deep copy of options.
	/// </summary>
	public RequestClientOptions Clone()
	{
		var clone = new RequestClientOptions
		{
			SessionId = SessionId,
			Timeout = Timeout,
			BrowserType = BrowserType,
			ProxyUrl = ProxyUrl,
			IsRotatingProxy = IsRotatingProxy,
			LocalAddress = LocalAddress,
			DisableIPv4 = DisableIPv4,
			DisableIPv6 = DisableIPv6,
			ForceHttp1 = ForceHttp1,
			DisableHttp3 = DisableHttp3,
			WithProtocolRacing = WithProtocolRacing,
			InsecureSkipVerify = InsecureSkipVerify,
			WithRandomTlsExtensionOrder = WithRandomTlsExtensionOrder,
			WithCustomCookieJar = WithCustomCookieJar,
			WithoutCookieJar = WithoutCookieJar,
			WithDebug = WithDebug,
			CatchPanics = CatchPanics,
			CustomRequestClient = CustomRequestClient,
			TransportOptions = TransportOptions,
		};

		foreach (var header in DefaultHeaders)
		{
			clone.DefaultHeaders[header.Key] = [.. header.Value];
		}

		foreach (var host in CertificatePinningHosts)
		{
			clone.CertificatePinningHosts[host.Key] = [.. host.Value];
		}

		clone.HeaderOrder.AddRange(HeaderOrder);

		return clone;
	}
}