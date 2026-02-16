using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Http.TLS.Core;

/// <summary>
/// Custom TLS client configuration.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>CustomTlsClient</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L93">CustomTlsClient</a>
/// </remarks>
public class CustomRequestClient
{
	/// <summary>
	/// HTTP/2 settings.
	/// </summary>
	[JsonPropertyName("h2Settings")]
	public Dictionary<string, uint> Http2Settings { get; set; } = [];

	/// <summary>
	/// HTTP/2 settings order.
	/// </summary>
	[JsonPropertyName("h2SettingsOrder")]
	public List<string> Http2SettingsOrder { get; set; } = [];

	/// <summary>
	/// HTTP/3 settings.
	/// </summary>
	[JsonPropertyName("h3Settings")]
	public Dictionary<string, ulong> Http3Settings { get; set; } = [];

	/// <summary>
	/// HTTP/3 settings order.
	/// </summary>
	[JsonPropertyName("h3SettingsOrder")]
	public List<string> Http3SettingsOrder { get; set; } = [];

	/// <summary>
	/// HTTP/3 pseudo-header order.
	/// </summary>
	[JsonPropertyName("h3PseudoHeaderOrder")]
	public List<string> Http3PseudoHeaderOrder { get; set; } = [];

	/// <summary>
	/// Header priority.
	/// </summary>
	[JsonPropertyName("headerPriority")]
	public PriorityParam? HeaderPriority { get; set; }

	/// <summary>
	/// Certificate compression algorithms.
	/// </summary>
	[JsonPropertyName("certCompressionAlgos")]
	public List<string> CertificateCompressionAlgorithms { get; set; } = [];

	/// <summary>
	/// JA3 fingerprint.
	/// </summary>
	[JsonPropertyName("ja3String")]
	public string Ja3Fingerprint { get; set; } = string.Empty;

	/// <summary>
	/// Key share curves.
	/// </summary>
	[JsonPropertyName("keyShareCurves")]
	public List<string> KeyShareCurves { get; set; } = [];

	/// <summary>
	/// ALPN protocols.
	/// </summary>
	[JsonPropertyName("alpnProtocols")]
	public List<string> AlpnProtocols { get; set; } = [];

	/// <summary>
	/// ALPS protocols.
	/// </summary>
	[JsonPropertyName("alpsProtocols")]
	public List<string> AlpsProtocols { get; set; } = [];

	/// <summary>
	/// ECH candidate payloads.
	/// </summary>
	[JsonPropertyName("ECHCandidatePayloads")]
	public List<ushort> EchCandidatePayloads { get; set; } = [];

	/// <summary>
	/// ECH candidate cipher suites.
	/// </summary>
	[JsonPropertyName("ECHCandidateCipherSuites")]
	public List<CandidateCipherSuite> EchCandidateCipherSuites { get; set; } = [];

	/// <summary>
	/// Priority frames.
	/// </summary>
	[JsonPropertyName("priorityFrames")]
	public List<PriorityFrame> PriorityFrames { get; set; } = [];

	/// <summary>
	/// Pseudo-header order.
	/// </summary>
	[JsonPropertyName("pseudoHeaderOrder")]
	public List<string> PseudoHeaderOrder { get; set; } = [];

	/// <summary>
	/// Supported delegated credentials algorithms.
	/// </summary>
	[JsonPropertyName("supportedDelegatedCredentialsAlgorithms")]
	public List<string> SupportedDelegatedCredentialsAlgorithms { get; set; } = [];

	/// <summary>
	/// Supported signature algorithms.
	/// </summary>
	[JsonPropertyName("supportedSignatureAlgorithms")]
	public List<string> SupportedSignatureAlgorithms { get; set; } = [];

	/// <summary>
	/// Supported versions.
	/// </summary>
	[JsonPropertyName("supportedVersions")]
	public List<string> SupportedVersions { get; set; } = [];

	/// <summary>
	/// Connection flow.
	/// </summary>
	[JsonPropertyName("connectionFlow")]
	public uint ConnectionFlow { get; set; }

	/// <summary>
	/// Record size limit.
	/// </summary>
	[JsonPropertyName("recordSizeLimit")]
	public ushort RecordSizeLimit { get; set; }

	/// <summary>
	/// Stream ID.
	/// </summary>
	[JsonPropertyName("streamId")]
	public uint StreamId { get; set; }

	/// <summary>
	/// HTTP/3 priority parameter.
	/// </summary>
	[JsonPropertyName("h3PriorityParam")]
	public uint Http3PriorityParam { get; set; }

	/// <summary>
	/// HTTP/3 send grease frames.
	/// </summary>
	[JsonPropertyName("h3SendGreaseFrames")]
	public bool Http3SendGreaseFrames { get; set; }

	/// <summary>
	/// Allow HTTP.
	/// </summary>
	[JsonPropertyName("allowHttp")]
	public bool AllowHttp { get; set; }
}