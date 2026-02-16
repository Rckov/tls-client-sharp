using Http.TLS.Utilities;

using System;
using System.Runtime.InteropServices;

namespace Http.TLS.Native;

internal static class NativeWrapper
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate IntPtr RequestDelegate(byte[] payload);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void FreeMemoryDelegate(string sessionId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate IntPtr GetCookiesFromSessionDelegate(byte[] payload);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate IntPtr AddCookiesToSessionDelegate(byte[] payload);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate IntPtr DestroySessionDelegate(byte[] payload);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate IntPtr DestroyAllDelegate();

	private static IntPtr _libraryHandle;
	private static RequestDelegate? _requestDelegate;
	private static FreeMemoryDelegate? _freeMemoryDelegate;
	private static GetCookiesFromSessionDelegate? _getCookiesDelegate;
	private static AddCookiesToSessionDelegate? _addCookiesDelegate;
	private static DestroySessionDelegate? _destroySessionDelegate;
	private static DestroyAllDelegate? _destroyAllDelegate;

	public static bool IsInitialized { get; private set; }

	public static void Initialize(string? path)
	{
		if (IsInitialized)
		{
			return;
		}

		_libraryHandle = NativeLoader.LoadLibrary(path.ThrowIfNotFileExists());

		try
		{
			_requestDelegate = GetDelegate<RequestDelegate>("request");
			_freeMemoryDelegate = GetDelegate<FreeMemoryDelegate>("freeMemory");
			_getCookiesDelegate = GetDelegate<GetCookiesFromSessionDelegate>("getCookiesFromSession");
			_addCookiesDelegate = GetDelegate<AddCookiesToSessionDelegate>("addCookiesToSession");
			_destroySessionDelegate = GetDelegate<DestroySessionDelegate>("destroySession");
			_destroyAllDelegate = GetDelegate<DestroyAllDelegate>("destroyAll");

			IsInitialized = true;
		}
		catch
		{
			NativeLoader.FreeLibrary(_libraryHandle);
			_libraryHandle = IntPtr.Zero;
			throw;
		}
	}

	public static string? Request(byte[] payload)
	{
		EnsureLoaded();
		return GetString(_requestDelegate!(payload));
	}

	public static string? GetCookiesFromSession(byte[] payload)
	{
		EnsureLoaded();
		return GetString(_getCookiesDelegate!(payload));
	}

	public static string? AddCookiesToSession(byte[] payload)
	{
		EnsureLoaded();
		return GetString(_addCookiesDelegate!(payload));
	}

	public static string? DestroySession(byte[] payload)
	{
		EnsureLoaded();
		return GetString(_destroySessionDelegate!(payload));
	}

	public static string? DestroyAllSessions()
	{
		EnsureLoaded();
		return GetString(_destroyAllDelegate!());
	}

	public static void FreeMemory(string responseId)
	{
		EnsureLoaded();
		_freeMemoryDelegate!(responseId);
	}

	public static void Cleanup()
	{
		if (_libraryHandle != IntPtr.Zero)
		{
			NativeLoader.FreeLibrary(_libraryHandle);
		}

		_libraryHandle = IntPtr.Zero;

		_requestDelegate = null;
		_freeMemoryDelegate = null;
		_getCookiesDelegate = null;
		_addCookiesDelegate = null;
		_destroySessionDelegate = null;
		_destroyAllDelegate = null;

		IsInitialized = false;
	}

	private static void EnsureLoaded()
	{
		if (!IsInitialized)
		{
			throw new InvalidOperationException("Native library is not initialized. Call RequestClient.Initialize(libraryPath) first.");
		}
	}

	private static T GetDelegate<T>(string name) where T : Delegate
	{
		var ptr = NativeLoader.GetProcAddress(_libraryHandle, name);
		if (ptr == IntPtr.Zero)
		{
			throw new EntryPointNotFoundException($"Function '{name}' not found in native library.");
		}

		return Marshal.GetDelegateForFunctionPointer<T>(ptr);
	}

	private static string? GetString(IntPtr ptr)
	{
		if (ptr == IntPtr.Zero)
		{
			throw new InvalidOperationException("Native function returned null pointer.");
		}

		return Marshal.PtrToStringAnsi(ptr);
	}
}