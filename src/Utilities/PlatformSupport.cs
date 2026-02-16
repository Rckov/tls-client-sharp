using System.Runtime.InteropServices;

namespace Http.TLS.Utilities;

public static class PlatformSupport
{
	public static Platform Platform =>
		RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? Platform.Osx :
		RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Platform.Linux :
		Platform.Windows;
}

public enum Platform : uint
{
	Linux,
	Osx,
	Windows
}