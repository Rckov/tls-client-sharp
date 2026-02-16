using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;

using System;
using System.Collections.Generic;

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
	/// <param name="request">The request to execute.</param>
	/// <returns>The <see cref="Response"/>; otherwise, <see langword="null"/>.</returns>
	Response? Send(Request? request);

	/// <summary>
	/// Retrieves cookies associated with the specified URI.
	/// </summary>
	/// <param name="uri">The URI to retrieve cookies for.</param>
	/// <returns>The <see cref="CookiesResponse"/>; otherwise, <see langword="null"/>.</returns>
	CookiesResponse? GetCookies(string uri);

	/// <summary>
	/// Associates cookies with the specified URI.
	/// </summary>
	/// <param name="uri">The URI to associate cookies with.</param>
	/// <param name="cookies">The cookies to associate.</param>
	/// <returns>The updated <see cref="CookiesResponse"/>; otherwise, <see langword="null"/>.</returns>
	CookiesResponse? AddCookies(string uri, List<ClientCookie> cookies);
}