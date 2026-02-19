using Http.TLS.Builders;
using Http.TLS.Core.Response;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Http.TLS.Extensions;

public static class RequestClientExtensions
{
	/// <summary>
	/// Sends a GET request to the specified URL.
	/// </summary>
	public static Task<Response?> GetAsync(
		this IRequestClient client,
		string url,
		CancellationToken cancellationToken = default) =>
		client.SendAsync(
			new RequestBuilder().WithUrl(url).WithMethod(HttpMethod.Get).Build(),
			cancellationToken);

	/// <summary>
	/// Sends a POST request with a JSON-serialized body.
	/// </summary>
	public static Task<Response?> PostJsonAsync<T>(
		this IRequestClient client,
		string url,
		T payload,
		CancellationToken cancellationToken = default) where T : class =>
		client.SendAsync(
			new RequestBuilder().WithUrl(url).WithMethod(HttpMethod.Post).WithBody(payload).Build(),
			cancellationToken);

	/// <summary>
	/// Sends a POST request with a string body.
	/// </summary>
	public static Task<Response?> PostAsync(
		this IRequestClient client,
		string url,
		string? body = null,
		CancellationToken cancellationToken = default) =>
		client.SendAsync(
			new RequestBuilder().WithUrl(url).WithMethod(HttpMethod.Post).WithBody(body).Build(),
			cancellationToken);

	/// <summary>
	/// Sends a PUT request with a JSON-serialized body.
	/// </summary>
	public static Task<Response?> PutJsonAsync<T>(
		this IRequestClient client,
		string url,
		T payload,
		CancellationToken cancellationToken = default) where T : class =>
		client.SendAsync(
			new RequestBuilder().WithUrl(url).WithMethod(HttpMethod.Put).WithBody(payload).Build(),
			cancellationToken);

	/// <summary>
	/// Sends a DELETE request to the specified URL.
	/// </summary>
	public static Task<Response?> DeleteAsync(
		this IRequestClient client,
		string url,
		CancellationToken cancellationToken = default) =>
		client.SendAsync(
			new RequestBuilder().WithUrl(url).WithMethod(HttpMethod.Delete).Build(),
			cancellationToken);
}