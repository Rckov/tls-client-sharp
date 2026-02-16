using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Response;

/// <summary>
/// Session cookies response.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>CookiesFromSessionOutput</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L45">CookiesFromSessionOutput</a>
/// </remarks>
public class CookiesResponse
{
	/// <summary>
	/// Response identifier.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Cookies.
	/// </summary>
	[JsonPropertyName("cookies")]
	public List<ClientCookie> Cookies { get; set; } = [];
}