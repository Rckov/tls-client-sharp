using System;
using System.Text.Json.Serialization;

namespace Http.TLS.Core.Request;

/// <summary>
/// Destroys a session.
/// </summary>
public class DestroySessionRequest
{
	[JsonPropertyName("sessionId")]
	public Guid SessionId { get; set; }
}