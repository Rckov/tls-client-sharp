using System.Text.Json.Serialization;

namespace Http.TLS.Core;

/// <summary>
/// HTTP/2 priority frame.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>PriorityFrames</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L154">PriorityFrames</a>
/// </remarks>
public class PriorityFrame
{
	/// <summary>
	/// Priority.
	/// </summary>
	[JsonPropertyName("priorityParam")]
	public PriorityParam PriorityParam { get; set; } = new();

	/// <summary>
	/// Stream ID.
	/// </summary>
	[JsonPropertyName("streamID")]
	public uint StreamId { get; set; }
}

/// <summary>
/// HTTP/2 priority parameters.
/// </summary>
/// <remarks>
/// Corresponds to Go struct <c>PriorityParam</c>:
/// <a href="https://github.com/bogdanfinn/tls-client/blob/master/cffi_src/types.go#L159">PriorityParam</a>
/// </remarks>
public class PriorityParam
{
	/// <summary>
	/// Stream dependency.
	/// </summary>
	[JsonPropertyName("streamDep")]
	public uint StreamDep { get; set; }

	/// <summary>
	/// Exclusive.
	/// </summary>
	[JsonPropertyName("exclusive")]
	public bool Exclusive { get; set; }

	/// <summary>
	/// Weight (1-256).
	/// </summary>
	[JsonPropertyName("weight")]
	public byte Weight { get; set; }
}