using Http.TLS.Extensions;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Response;

/// <summary>
/// HTTP response.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>Response</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L198">types.go#L198</a>
/// </remarks>
public class Response
{
	/// <summary>
	/// Response identifier.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// HTTP status code.
	/// </summary>
	[JsonPropertyName("status")]
	public int Status { get; set; }

	/// <summary>
	/// Response body.
	/// </summary>
	[JsonPropertyName("body")]
	public string Body { get; set; } = string.Empty;

	/// <summary>
	/// Final URL after redirects.
	/// </summary>
	[JsonPropertyName("target")]
	public string Target { get; set; } = string.Empty;

	/// <summary>
	/// Protocol version.
	/// </summary>
	[JsonPropertyName("usedProtocol")]
	public string UsedProtocol { get; set; } = string.Empty;

	/// <summary>
	/// Response headers.
	/// </summary>
	[JsonPropertyName("headers")]
	public Dictionary<string, List<string>> Headers { get; set; } = [];

	/// <summary>
	/// First header value or null.
	/// </summary>
	public string? GetHeader(string name) => Headers.GetHeader(name);

	/// <summary>
	/// All header values.
	/// </summary>
	public List<string> GetHeaderValues(string name) => Headers.GetHeaderValues(name);

	/// <summary>
	/// Checks header existence.
	/// </summary>
	public bool HasHeader(string name) => Headers.HasHeader(name);

	/// <summary>
	/// Cookies.
	/// </summary>
	[JsonPropertyName("cookies")]
	public Dictionary<string, string> Cookies { get; set; } = [];

	/// <summary>
	/// Cookie value or null.
	/// </summary>
	public string? GetCookie(string name) => Cookies.TryGetValue(name, out var value) ? value : null;

	/// <summary>
	/// Checks cookie existence.
	/// </summary>
	public bool HasCookie(string name) => Cookies.ContainsKey(name);

	/// <summary>
	/// Session identifier.
	/// </summary>
	[JsonPropertyName("sessionId")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? SessionId { get; set; }

	/// <summary>
	/// True for 2xx status codes.
	/// </summary>
	public bool IsSuccessStatus => Status is >= 200 and < 300;

	/// <summary>
	/// Content-Type header.
	/// </summary>
	public string? ContentType => GetHeader("Content-Type");

	/// <summary>
	/// Content length or -1 if missing.
	/// </summary>
	public long ContentLength
	{
		get
		{
			var header = GetHeader("Content-Length");
			return long.TryParse(header, out var length) ? length : -1;
		}
	}
}