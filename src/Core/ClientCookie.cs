using System.Text.Json.Serialization;

namespace Http.TLS.Core;

/// <summary>
/// HTTP cookie.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>Cookie</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L165">Cookie</a>
/// </remarks>
public class ClientCookie
{
	/// <summary>
	/// Name.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Value.
	/// </summary>
	[JsonPropertyName("value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// Domain.
	/// </summary>
	[JsonPropertyName("domain")]
	public string Domain { get; set; } = string.Empty;

	/// <summary>
	/// Path.
	/// </summary>
	[JsonPropertyName("path")]
	public string Path { get; set; } = string.Empty;

	/// <summary>
	/// Expiration timestamp. 0 for session cookie.
	/// </summary>
	[JsonPropertyName("expires")]
	public long Expires { get; set; } = 0;

	/// <summary>
	/// Max age in seconds.
	/// </summary>
	[JsonPropertyName("maxAge")]
	public int MaxAge { get; set; } = 0;

	/// <summary>
	/// HTTPS only.
	/// </summary>
	[JsonPropertyName("secure")]
	public bool Secure { get; set; } = false;

	/// <summary>
	/// HTTP only.
	/// </summary>
	[JsonPropertyName("httpOnly")]
	public bool HttpOnly { get; set; } = false;

	/// <summary>
	/// Creates cookie.
	/// </summary>
	public ClientCookie(string name, string value)
	{
		Name = name;
		Value = value;
	}

	/// <summary>
	/// Default constructor.
	/// </summary>
	public ClientCookie()
	{ }
}