using System;

namespace Http.TLS.Native;

/// <summary>
/// Manages the lifetime of the native TLS library.
/// Create once at application startup and dispose on shutdown.
/// </summary>
public sealed class NativeTlsClientContext : IDisposable
{
    private bool _disposed;

    public NativeTlsClientContext(string libraryPath)
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
