using Http.TLS.Utilities;

using System;
using System.Runtime.InteropServices;

namespace Http.TLS.Native;

internal static partial class NativeLoader
{
	public static IntPtr LoadLibrary(string? path)
	{
		return PlatformSupport.Platform switch
		{
			Platform.Linux => Linux.LoadLibrary(path),
			Platform.Windows => Windows.LoadLibrary(path),
			Platform.Osx => MacOs.LoadLibrary(path),
			_ => throw new PlatformNotSupportedException()
		};
	}

	public static IntPtr GetProcAddress(IntPtr handle, string name)
	{
		return PlatformSupport.Platform switch
		{
			Platform.Linux => Linux.GetProcAddress(handle, name),
			Platform.Windows => Windows.GetProcAddress(handle, name),
			Platform.Osx => MacOs.GetProcAddress(handle, name),
			_ => throw new PlatformNotSupportedException()
		};
	}

	public static bool FreeLibrary(IntPtr handle)
	{
		return PlatformSupport.Platform switch
		{
			Platform.Linux => Linux.FreeLibrary(handle) == 0,
			Platform.Windows => Windows.FreeLibrary(handle),
			Platform.Osx => MacOs.FreeLibrary(handle) == 0,
			_ => throw new PlatformNotSupportedException()
		};
	}

	private static partial class Windows
	{
#if NET8_0_OR_GREATER
		[LibraryImport("kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
		public static partial IntPtr LoadLibrary(string? path);

		[LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
		public static partial IntPtr GetProcAddress(IntPtr hModule, string procName);

		[LibraryImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool FreeLibrary(IntPtr hLibrary);
#else

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, EntryPoint = "LoadLibraryW")]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)] string? path);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hLibrary);

#endif
	}

	private static partial class Linux
	{
		[Flags]
		public enum LoadFlags
		{
			None = 0,
			Now = 1 << 1,
			Global = 1 << 8
		}

#if NET8_0_OR_GREATER
		[LibraryImport("libdl.so.2", EntryPoint = "dlopen", StringMarshalling = StringMarshalling.Utf8)]
		public static partial IntPtr LoadLibrary(string? path, LoadFlags flags = LoadFlags.Now | LoadFlags.Global);

		[LibraryImport("libdl.so.2", EntryPoint = "dlsym", StringMarshalling = StringMarshalling.Utf8)]
		public static partial IntPtr GetProcAddress(IntPtr handle, string symbol);

		[LibraryImport("libdl.so.2", EntryPoint = "dlclose")]
		public static partial int FreeLibrary(IntPtr handle);
#else

		[DllImport("libdl.so.2", EntryPoint = "dlopen")]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string? path, LoadFlags flags = LoadFlags.Now | LoadFlags.Global);

		[DllImport("libdl.so.2", EntryPoint = "dlsym")]
		public static extern IntPtr GetProcAddress(IntPtr handle, string symbol);

		[DllImport("libdl.so.2", EntryPoint = "dlclose")]
		public static extern int FreeLibrary(IntPtr handle);

#endif
	}

	private static partial class MacOs
	{
		[Flags]
		public enum LoadFlags
		{
			None = 0,
			Now = 1 << 1,
			Global = 1 << 3
		}

#if NET8_0_OR_GREATER
		[LibraryImport("libdl.dylib", EntryPoint = "dlopen", StringMarshalling = StringMarshalling.Utf8)]
		public static partial IntPtr LoadLibrary(string? path, LoadFlags flags = LoadFlags.Now | LoadFlags.Global);

		[LibraryImport("libdl.dylib", EntryPoint = "dlsym", StringMarshalling = StringMarshalling.Utf8)]
		public static partial IntPtr GetProcAddress(IntPtr handle, string symbol);

		[LibraryImport("libdl.dylib", EntryPoint = "dlclose")]
		public static partial int FreeLibrary(IntPtr handle);
#else

		[DllImport("libdl.dylib", EntryPoint = "dlopen")]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string? path, LoadFlags flags = LoadFlags.Now | LoadFlags.Global);

		[DllImport("libdl.dylib", EntryPoint = "dlsym")]
		public static extern IntPtr GetProcAddress(IntPtr handle, string symbol);

		[DllImport("libdl.dylib", EntryPoint = "dlclose")]
		public static extern int FreeLibrary(IntPtr handle);

#endif
	}
}