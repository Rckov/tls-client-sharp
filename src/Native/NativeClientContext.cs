using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Http.TLS.Native;

/// <summary>
/// Manages the lifetime of the native TLS library.
/// Create once at application startup and dispose on shutdown.
/// </summary>
public sealed class NativeClientContext : IDisposable
{
	private bool _disposed;

	public bool IsInitialized => NativeWrapper.IsInitialized;

	public NativeClientContext(string libraryPath)
	{
		NativeWrapper.Initialize(libraryPath);
	}

#if NET8_0_OR_GREATER
	internal static readonly List<JsonSerializerContext> RegisteredContexts = [];

	/// <summary>
	/// Registers a <see cref="JsonSerializerContext"/> for AOT-safe serialization of custom types.
	/// </summary>
	public void RegisterContext(JsonSerializerContext context)
	{
		RegisteredContexts.Add(context);
	}
#endif

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		NativeWrapper.Cleanup();
	}
}