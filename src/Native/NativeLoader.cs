using Http.TLS.Utilities;

using System;
using System.Runtime.InteropServices;

namespace Http.TLS.Native;

internal static class NativeLoader
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

	private static class Windows
	{
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, EntryPoint = "LoadLibraryW")]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)] string? path);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hLibrary);
	}

	private static class Linux
	{
		[Flags]
		public enum LoadFlags
		{
			Now = 1 << 1,
			Global = 1 << 8
		}

		[DllImport("libdl.so.2", EntryPoint = "dlopen")]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string? path, LoadFlags flags = LoadFlags.Now | LoadFlags.Global);

		[DllImport("libdl.so.2", EntryPoint = "dlsym")]
		public static extern IntPtr GetProcAddress(IntPtr handle, string symbol);

		[DllImport("libdl.so.2", EntryPoint = "dlclose")]
		public static extern int FreeLibrary(IntPtr handle);
	}

	private static class MacOs
	{
		[Flags]
		public enum LoadFlags
		{
			Now = 1 << 1,
			Global = 1 << 3
		}

		[DllImport("libdl.dylib", EntryPoint = "dlopen")]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string? path, LoadFlags flags = LoadFlags.Now | LoadFlags.Global);

		[DllImport("libdl.dylib", EntryPoint = "dlsym")]
		public static extern IntPtr GetProcAddress(IntPtr handle, string symbol);

		[DllImport("libdl.dylib", EntryPoint = "dlclose")]
		public static extern int FreeLibrary(IntPtr handle);
	}
}