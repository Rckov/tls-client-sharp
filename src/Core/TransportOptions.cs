using System.Text.Json.Serialization;

namespace Http.TLS.Core;

/// <summary>
/// Transport options.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>TransportOptions</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L140">TransportOptions</a>
/// </remarks>
public class TransportOptions
{
	/// <summary>
	/// Idle connection timeout.
	/// </summary>
	[JsonPropertyName("idleConnTimeout")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public long? IdleConnTimeout { get; set; }

	/// <summary>
	/// Max idle connections.
	/// </summary>
	[JsonPropertyName("maxIdleConns")]
	public int MaxIdleConns { get; set; }

	/// <summary>
	/// Max idle connections per host.
	/// </summary>
	[JsonPropertyName("maxIdleConnsPerHost")]
	public int MaxIdleConnsPerHost { get; set; }

	/// <summary>
	/// Max connections per host.
	/// </summary>
	[JsonPropertyName("maxConnsPerHost")]
	public int MaxConnsPerHost { get; set; }

	/// <summary>
	/// Max response header bytes.
	/// </summary>
	[JsonPropertyName("maxResponseHeaderBytes")]
	public long MaxResponseHeaderBytes { get; set; }

	/// <summary>
	/// Write buffer size.
	/// </summary>
	[JsonPropertyName("writeBufferSize")]
	public int WriteBufferSize { get; set; }

	/// <summary>
	/// Read buffer size.
	/// </summary>
	[JsonPropertyName("readBufferSize")]
	public int ReadBufferSize { get; set; }

	/// <summary>
	/// Disable keep-alives.
	/// </summary>
	[JsonPropertyName("disableKeepAlives")]
	public bool DisableKeepAlives { get; set; }

	/// <summary>
	/// Disable compression.
	/// </summary>
	[JsonPropertyName("disableCompression")]
	public bool DisableCompression { get; set; }
}