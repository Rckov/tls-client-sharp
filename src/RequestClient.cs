using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;
using Http.TLS.Native;
using Http.TLS.Utilities;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Http.TLS;

public sealed class RequestClient(RequestClientOptions? options) : IRequestClient
{
	/// <inheritdoc />
	public RequestClientOptions Options
		=> options.Pipe(o => o.Validate(), nameof(options));

	/// <inheritdoc />
	public Guid SessionId => Options.SessionId;

	/// <inheritdoc />
	public Response? Send(Request? request)
	{
		request.ThrowIfNull(nameof(request));
		request!.RequestUrl.ThrowIfNullOrEmpty(nameof(request.RequestUrl));

		Response? response = null;
		var prepared = PrepareRequest(request);

		try
		{
			var responseJson = NativeWrapper.Request(Serializer.SerializeToBytes(prepared));
			response = Serializer.Deserialize<Response>(responseJson);
			return response;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Request failed: {ex.Message}", ex);
		}
		finally
		{
			if (response != null && !string.IsNullOrEmpty(response.Id))
			{
				NativeWrapper.FreeMemory(response.Id);
			}
		}
	}

	/// <inheritdoc />
	public CookiesResponse? GetCookies(string uri)
	{
		uri.ThrowIfNullOrEmpty(nameof(uri));

		var payload = new GetCookiesRequest
		{
			Url = uri,
			SessionId = Options.SessionId,
		};

		var responseJson = NativeWrapper.GetCookiesFromSession(Serializer.SerializeToBytes(payload));
		return Serializer.Deserialize<CookiesResponse>(responseJson);
	}

	/// <inheritdoc />
	public CookiesResponse? AddCookies(string uri, List<ClientCookie> cookies)
	{
		uri.ThrowIfNullOrEmpty(nameof(uri));
		cookies.ThrowIfNull(nameof(cookies));

		var payload = new AddCookiesRequest
		{
			Url = uri,
			SessionId = Options.SessionId,
			Cookies = cookies
		};

		var responseJson = NativeWrapper.AddCookiesToSession(Serializer.SerializeToBytes(payload));
		return Serializer.Deserialize<CookiesResponse>(responseJson);
	}

	/// <inheritdoc />
	public Task<Response?> SendAsync(Request? request, CancellationToken cancellationToken = default)
	{
		return Task.Run(() => Send(request), cancellationToken);
	}

	/// <inheritdoc />
	public Task<CookiesResponse?> GetCookiesAsync(string uri, CancellationToken cancellationToken = default)
	{
		return Task.Run(() => GetCookies(uri), cancellationToken);
	}

	/// <inheritdoc />
	public Task<CookiesResponse?> AddCookiesAsync(string uri, List<ClientCookie> cookies, CancellationToken cancellationToken = default)
	{
		return Task.Run(() => AddCookies(uri, cookies), cancellationToken);
	}

	/// <inheritdoc />
	public void Dispose()
	{
		if (!NativeWrapper.IsInitialized)
		{
			return;
		}

		try
		{
			var payload = new { sessionId = Options.SessionId };
			NativeWrapper.DestroySession(Serializer.SerializeToBytes(payload));
		}
		catch (InvalidOperationException)
		{
		}
	}

	private Request PrepareRequest(Request request)
	{
		var prepared = request.Clone();
		ApplyClientDefaults(prepared);
		return prepared;
	}

	private void ApplyClientDefaults(Request request)
	{
		request.SessionId = Options.SessionId;

		if (request.TimeoutMilliseconds == 0)
		{
			request.TimeoutMilliseconds = (int)Options.Timeout.TotalMilliseconds;
		}

		request.ProxyUrl ??= Options.ProxyUrl;
		request.LocalAddress ??= Options.LocalAddress;
		request.CustomRequestClient ??= Options.CustomRequestClient;
		request.TransportOptions ??= Options.TransportOptions;
		request.BrowserType ??= Options.BrowserType;

		request.InsecureSkipVerify ??= Options.InsecureSkipVerify;
		request.WithRandomTlsExtensionOrder ??= Options.WithRandomTlsExtensionOrder;
		request.DisableIPv4 ??= Options.DisableIPv4;
		request.DisableIPv6 ??= Options.DisableIPv6;
		request.ForceHttp1 ??= Options.ForceHttp1;
		request.DisableHttp3 ??= Options.DisableHttp3;
		request.WithProtocolRacing ??= Options.WithProtocolRacing;
		request.WithDebug ??= Options.WithDebug;
		request.CatchPanics ??= Options.CatchPanics;
		request.IsRotatingProxy ??= Options.IsRotatingProxy;
		request.WithCustomCookieJar ??= Options.WithCustomCookieJar;
		request.WithoutCookieJar ??= Options.WithoutCookieJar;

		foreach (var header in Options.DefaultHeaders)
		{
			if (!request.DefaultHeaders.ContainsKey(header.Key))
			{
				request.DefaultHeaders[header.Key] = [.. header.Value];
			}

			if (!request.Headers.ContainsKey(header.Key) && header.Value.Count > 0)
			{
				request.Headers[header.Key] = header.Value[0];
			}
		}

		foreach (var host in Options.CertificatePinningHosts)
		{
			if (!request.CertificatePinningHosts.ContainsKey(host.Key))
			{
				request.CertificatePinningHosts[host.Key] = [.. host.Value];
			}
		}

		if (request.HeaderOrder.Count == 0 && Options.HeaderOrder.Count > 0)
		{
			request.HeaderOrder.AddRange(Options.HeaderOrder);
		}
	}
}