using Http.TLS.Utilities;

using System.Collections.Generic;
using System.Linq;

namespace Http.TLS.Extensions;

public static class HeadersExtensions
{
	extension(Dictionary<string, string> headers)
	{
		/// <summary>
		/// Adds or replaces header.
		/// </summary>
		public Dictionary<string, string> WithHeader(string name, string value)
		{
			headers[name.ThrowIfNullOrEmpty()] = value;
			return headers;
		}

		/// <summary>
		/// Gets header value or null.
		/// </summary>
		public string? GetHeader(string name) =>
			headers.TryGetValue(name, out var value) ? value : null;

		/// <summary>
		/// Checks header existence.
		/// </summary>
		public bool HasHeader(string name) => headers.ContainsKey(name);

		/// <summary>
		/// Removes header.
		/// </summary>
		public Dictionary<string, string> WithoutHeader(string name)
		{
			headers.Remove(name);
			return headers;
		}
	}

	extension(Dictionary<string, List<string>> headers)
	{
		/// <summary>
		/// Adds or replaces header values.
		/// </summary>
		public Dictionary<string, List<string>> WithHeader(string name, params string[] values)
		{
			headers[name.ThrowIfNullOrEmpty()] = [.. values];
			return headers;
		}

		/// <summary>
		/// Appends values to existing header.
		/// </summary>
		public Dictionary<string, List<string>> AddHeader(string name, params string[] values)
		{
			if (!headers.TryGetValue(name, out var list))
			{
				headers[name] = list = [];
			}

			list.AddRange(values);
			return headers;
		}

		/// <summary>
		/// Gets first header value or null.
		/// </summary>
		public string? GetHeader(string name) =>
			headers.TryGetValue(name, out var values) && values.Count > 0 ? values[0] : null;

		/// <summary>
		/// Gets all header values or empty list.
		/// </summary>
		public List<string> GetHeaderValues(string name) =>
			headers.TryGetValue(name, out var values) ? values : [];

		/// <summary>
		/// Checks header existence and non-empty values.
		/// </summary>
		public bool HasHeader(string name) =>
			headers.TryGetValue(name, out var values) && values.Count > 0;

		/// <summary>
		/// Converts to raw HTTP headers.
		/// </summary>
		public string ToRawHeaders() =>
			string.Join("\r\n", headers.SelectMany(kvp => kvp.Value.Select(v => $"{kvp.Key}: {v}")));
	}
}