using System.Text.Json.Serialization;

namespace Http.TLS.Core;

/// <summary>
/// ECH cipher suite.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>CandidateCipherSuite</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L134">CandidateCipherSuite</a>
/// </remarks>
public class CandidateCipherSuite
{
	/// <summary>
	/// KDF identifier.
	/// </summary>
	[JsonPropertyName("kdfId")]
	public string KdfId { get; set; } = string.Empty;

	/// <summary>
	/// AEAD identifier.
	/// </summary>
	[JsonPropertyName("aeadId")]
	public string AeadId { get; set; } = string.Empty;
}