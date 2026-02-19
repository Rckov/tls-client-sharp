using System;

namespace Http.TLS.Native;

/// <summary>
/// Manages the lifetime of the native TLS library.
/// Create once at application startup and dispose on shutdown.
/// </summary>
public sealed class NativeClientContext : IDisposable
{
	private bool _disposed;

	public NativeClientContext(string libraryPath)
	{
		NativeWrapper.Initialize(libraryPath);
	}

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