using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Request;

/// <summary>
/// Adds cookies to session.
/// </summary>
/// <remarks>
/// Corresponds to the Go <c>AddCookiesToSessionInput</c> struct:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L34">AddCookiesToSessionInput</a>
/// </remarks>
public class AddCookiesRequest
{
	/// <summary>
	/// Session identifier.
	/// </summary>
	[JsonPropertyName("sessionId")]
	public Guid SessionId { get; set; }

	/// <summary>
	/// Target URL.
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;

	/// <summary>
	/// Cookies.
	/// </summary>
	[JsonPropertyName("cookies")]
	public List<ClientCookie> Cookies { get; set; } = [];
}

/// <summary>
/// Gets cookies from session.
/// </summary>
/// <remarks>
/// Corresponds to the Go <c>GetCookiesFromSessionInput</c> struct:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L40">GetCookiesFromSessionInput</a>
/// </remarks>
public class GetCookiesRequest
{
	/// <summary>
	/// Session identifier.
	/// </summary>
	[JsonPropertyName("sessionId")]
	public Guid SessionId { get; set; }

	/// <summary>
	/// Target URL.
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;
}