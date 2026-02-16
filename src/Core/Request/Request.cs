using Http.TLS.Core.Converters;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Request;

/// <summary>
/// HTTP request configuration.
/// </summary>
/// <remarks>
/// Corresponds to the Go <c>RequestInput</c> struct:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L51">RequestInput</a>
/// </remarks>
public class Request
{
	/// <summary>
	/// Request URL.
	/// </summary>
	[JsonPropertyName("requestUrl")]
	public string RequestUrl { get; set; } = string.Empty;

	/// <summary>
	/// HTTP method.
	/// </summary>
	[JsonPropertyName("requestMethod")]
	public string RequestMethod { get; set; } = "GET";

	/// <summary>
	/// Session identifier.
	/// </summary>
	[JsonPropertyName("sessionId")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Guid? SessionId { get; set; }

	/// <summary>
	/// Request headers.
	/// </summary>
	[JsonPropertyName("headers")]
	public Dictionary<string, string> Headers { get; set; } = [];

	/// <summary>
	/// Default headers.
	/// </summary>
	[JsonPropertyName("defaultHeaders")]
	public Dictionary<string, List<string>> DefaultHeaders { get; set; } = [];

	/// <summary>
	/// CONNECT headers.
	/// </summary>
	[JsonPropertyName("connectHeaders")]
	public Dictionary<string, List<string>> ConnectHeaders { get; set; } = [];

	/// <summary>
	/// Header order.
	/// </summary>
	[JsonPropertyName("headerOrder")]
	public List<string> HeaderOrder { get; set; } = [];

	/// <summary>
	/// Request body.
	/// </summary>
	[JsonPropertyName("requestBody")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? RequestBody { get; set; }

	/// <summary>
	/// Binary body flag.
	/// </summary>
	[JsonPropertyName("isByteRequest")]
	public bool IsByteRequest { get; set; }

	/// <summary>
	/// Request cookies.
	/// </summary>
	[JsonPropertyName("requestCookies")]
	public List<ClientCookie> RequestCookies { get; set; } = [];

	/// <summary>
	/// Custom cookie jar.
	/// </summary>
	[JsonPropertyName("withCustomCookieJar")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? WithCustomCookieJar { get; set; }

	/// <summary>
	/// Disables cookie handling.
	/// </summary>
	[JsonPropertyName("withoutCookieJar")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? WithoutCookieJar { get; set; }

	/// <summary>
	/// Proxy URL.
	/// </summary>
	[JsonPropertyName("proxyUrl")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? ProxyUrl { get; set; }

	/// <summary>
	/// Rotating proxy flag.
	/// </summary>
	[JsonPropertyName("isRotatingProxy")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? IsRotatingProxy { get; set; }

	/// <summary>
	/// Browser fingerprint.
	/// </summary>
	[JsonPropertyName("tlsClientIdentifier")]
	public BrowserType? BrowserType { get; set; }

	/// <summary>
	/// SNI hostname override.
	/// </summary>
	[JsonPropertyName("serverNameOverwrite")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? ServerNameOverwrite { get; set; }

	/// <summary>
	/// Host header override.
	/// </summary>
	[JsonPropertyName("requestHostOverride")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? RequestHostOverride { get; set; }

	/// <summary>
	/// Skips SSL verification.
	/// </summary>
	[JsonPropertyName("insecureSkipVerify")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? InsecureSkipVerify { get; set; }

	/// <summary>
	/// Randomizes TLS extensions.
	/// </summary>
	[JsonPropertyName("withRandomTLSExtensionOrder")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? WithRandomTlsExtensionOrder { get; set; }

	/// <summary>
	/// Certificate pinning.
	/// </summary>
	[JsonPropertyName("certificatePinningHosts")]
	public Dictionary<string, List<string>> CertificatePinningHosts { get; set; } = [];

	/// <summary>
	/// Local IP address.
	/// </summary>
	[JsonPropertyName("localAddress")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? LocalAddress { get; set; }

	/// <summary>
	/// Disables IPv4.
	/// </summary>
	[JsonPropertyName("disableIPV4")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? DisableIPv4 { get; set; }

	/// <summary>
	/// Disables IPv6.
	/// </summary>
	[JsonPropertyName("disableIPV6")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? DisableIPv6 { get; set; }

	/// <summary>
	/// Forces HTTP/1.1.
	/// </summary>
	[JsonPropertyName("forceHttp1")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? ForceHttp1 { get; set; }

	/// <summary>
	/// Disables HTTP/3.
	/// </summary>
	[JsonPropertyName("disableHttp3")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? DisableHttp3 { get; set; }

	/// <summary>
	/// Enables protocol racing.
	/// </summary>
	[JsonPropertyName("withProtocolRacing")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? WithProtocolRacing { get; set; }

	/// <summary>
	/// Timeout in milliseconds.
	/// </summary>
	[JsonPropertyName("timeoutMilliseconds")]
	public int TimeoutMilliseconds { get; set; }

	/// <summary>
	/// Timeout in seconds.
	/// </summary>
	[JsonPropertyName("timeoutSeconds")]
	public int TimeoutSeconds { get; set; }

	/// <summary>
	/// Binary response flag.
	/// </summary>
	[JsonPropertyName("isByteResponse")]
	public bool IsByteResponse { get; set; }

	/// <summary>
	/// EUC-KR response encoding.
	/// </summary>
	[JsonPropertyName("euckrResponse")]
	public bool EuckrResponse { get; set; }

	/// <summary>
	/// Follows redirects.
	/// </summary>
	[JsonPropertyName("followRedirects")]
	public bool FollowRedirects { get; set; }

	/// <summary>
	/// Stream output path.
	/// </summary>
	[JsonPropertyName("streamOutputPath")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? StreamOutputPath { get; set; }

	/// <summary>
	/// Stream block size.
	/// </summary>
	[JsonPropertyName("streamOutputBlockSize")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? StreamOutputBlockSize { get; set; }

	/// <summary>
	/// Stream EOF symbol.
	/// </summary>
	[JsonPropertyName("streamOutputEOFSymbol")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? StreamOutputEofSymbol { get; set; }

	/// <summary>
	/// Debug mode.
	/// </summary>
	[JsonPropertyName("withDebug")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? WithDebug { get; set; }

	/// <summary>
	/// Catches panics.
	/// </summary>
	[JsonPropertyName("catchPanics")]
	[JsonConverter(typeof(NullableBoolToFalseConverter))]
	public bool? CatchPanics { get; set; }

	/// <summary>
	/// Custom TLS client.
	/// </summary>
	[JsonPropertyName("customTlsClient")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public CustomRequestClient? CustomRequestClient { get; set; }

	/// <summary>
	/// Transport options.
	/// </summary>
	[JsonPropertyName("transportOptions")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public TransportOptions? TransportOptions { get; set; }
}