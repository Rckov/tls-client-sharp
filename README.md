[![Target Frameworks](https://img.shields.io/badge/Target%20Frameworks-netstandard2.0%20%7C%20net5.0%20%7C%20net6.0%20%7C%20net8.0%20%7C%20net9.0%20%7C%20net10.0-512BD4)]()
[![Build and Release](https://github.com/Rckov/tls-client-sharp/actions/workflows/release.yml/badge.svg)](https://github.com/Rckov/tls-client-sharp/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/Http.TLS.svg?label=NuGet)](https://www.nuget.org/packages/Http.TLS)

# Http.TLS

.NET library for HTTP/2 HTTP/3 requests with TLS fingerprinting support. Based on [bogdanfinn/tls-client](https://github.com/bogdanfinn/tls-client/), it allows you to mimic various browser fingerprints to bypass bot protection.

## Installation

```bash
dotnet add package Http.TLS
```

### Native Library

Download the appropriate native library for your platform from the [bogdanfinn/tls-client releases page](https://github.com/bogdanfinn/tls-client/releases/latest).

**Initialization:**

```csharp
// Initialize once at application startup, dispose on shutdown
using var context = new NativeClientContext("path/to/native-library");
```

## Quick Start

```csharp
using var context = new NativeClientContext("tls-client-windows-64-1.14.0.dll");

using var client = new RequestClientBuilder()
    .WithBrowserType(BrowserType.Chrome133)
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();

var request = new RequestBuilder()
    .WithUrl("https://httpbin.org/get")
    .Build();

var response = await client.SendAsync(request);
Console.WriteLine($"Status: {response?.Status}");
Console.WriteLine($"Body: {response?.Body}");
```

## RequestClientBuilder

Fluent API for client configuration:

```csharp
using var client = new RequestClientBuilder()
    .WithBrowserType(BrowserType.Chrome133)
    .WithTimeout(TimeSpan.FromSeconds(30))
    .WithProxy("http://proxy:8080")
    .WithCookieJar(true)
    .WithDebug(true)
    .Build();
```

**Available methods:**
- `WithBrowserType()` - browser fingerprint selection
- `WithTimeout()` - request timeout
- `WithProxy()` - proxy configuration
- `WithCookieJar()` / `WithoutCookieJar()` - cookie management
- `WithInsecureSkipVerify()` - skip SSL verification
- ...

## RequestClientFactory

Factory for managing multiple named client configurations:

```csharp
var factory = new RequestClientFactory(o => o.BrowserType = BrowserType.Chrome133);
factory.Register("mobile", o => o.BrowserType = BrowserType.SafariIos17_2);

using var defaultClient = factory.CreateClient();
using var mobileClient = factory.CreateClient("mobile");
using var customClient = factory.CreateClient(o => o.WithProxy("http://proxy:8080"));
```

## RequestBuilder

Fluent API for request creation:

```csharp
var request = new RequestBuilder()
    .WithUrl("https://api.example.com/data")
    .WithMethod(HttpMethod.Post)
    .WithBody(new { name = "John", age = 30 })
    .WithHeader("Authorization", "Bearer token")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

**Available methods:**
- `WithUrl()` - request URL
- `WithMethod()` - HTTP method
- `WithBody()` - request body (string, object, byte[])
- `WithHeader()` / `WithHeaders()` - headers
- `WithCookie()` / `WithCookies()` - cookies
- ...

## Features

- **HTTP/1.1, HTTP/2, HTTP/3** - Full protocol support with automatic negotiation
- **Protocol Racing** - Chrome-like "Happy Eyeballs" for HTTP/2 vs HTTP/3
- **TLS Fingerprinting** - Mimic Chrome, Firefox, Safari, and other browsers
- **HTTP/3 Fingerprinting** - Accurate QUIC/HTTP/3 fingerprints matching real browsers
- **Custom Header Ordering** - Control the order of HTTP headers
- **Proxy Support** - HTTP and SOCKS5 proxies
- **Cookie Jar Management** - Built-in cookie handling
- **Certificate Pinning** - Enhanced security with custom certificate validation
...

## Examples

### GET Request

```csharp
var request = new RequestBuilder()
    .WithUrl("https://httpbin.org/get")
    .WithMethod(HttpMethod.Get)
    .Build();

var response = await client.SendAsync(request);
```

### POST with JSON

```csharp
var request = new RequestBuilder()
    .WithUrl("https://httpbin.org/post")
    .WithMethod(HttpMethod.Post)
    .WithBody(new { name = "John", age = 30 })
    .Build();

var response = await client.SendAsync(request);
```

## Target Frameworks

- .NET Standard 2.0
- .NET 5.0, 6.0, 8.0, 9.0, 10.0

## License

[MIT License](LICENSE) | [Report an Issue](https://github.com/Rckov/tls-client-sharp/issues)
