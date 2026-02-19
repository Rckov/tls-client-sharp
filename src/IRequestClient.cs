using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Http.TLS;

public interface IRequestClient : IDisposable
{
	/// <summary>
	/// Unique session ID.
	/// </summary>
	Guid SessionId { get; }

	/// <summary>
	/// Client configuration. Read-only after building.
	/// </summary>
	RequestClientOptions Options { get; }

	/// <summary>
	/// Executes the specified HTTP request.
	/// </summary>
	Response? Send(Request? request);

	/// <summary>
	/// Executes the specified HTTP request asynchronously.
	/// </summary>
	Task<Response?> SendAsync(Request? request, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves cookies associated with the specified URI.
	/// </summary>
	CookiesResponse? GetCookies(string uri);

	/// <summary>
	/// Retrieves cookies associated with the specified URI asynchronously.
	/// </summary>
	Task<CookiesResponse?> GetCookiesAsync(string uri, CancellationToken cancellationToken = default);

	/// <summary>
	/// Associates cookies with the specified URI.
	/// </summary>
	CookiesResponse? AddCookies(string uri, List<ClientCookie> cookies);

	/// <summary>
	/// Associates cookies with the specified URI asynchronously.
	/// </summary>
	Task<CookiesResponse?> AddCookiesAsync(string uri, List<ClientCookie> cookies, CancellationToken cancellationToken = default);
}