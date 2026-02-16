using Http.TLS.Core;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Http.TLS.Extensions;

public static class CookieExtensions
{
	extension(ClientCookie cookie)
	{
		/// <summary>
		/// Converts to session cookie (no expiration).
		/// </summary>
		public ClientCookie AsSessionCookie()
		{
			cookie.Expires = 0;
			cookie.MaxAge = 0;
			return cookie;
		}

		/// <summary>
		/// Sets expiration timestamp.
		/// </summary>
		public ClientCookie WithExpiration(DateTime expires)
		{
			cookie.Expires = new DateTimeOffset(expires).ToUnixTimeSeconds();
			return cookie;
		}

		/// <summary>
		/// Sets max age.
		/// </summary>
		public ClientCookie WithMaxAge(TimeSpan maxAge)
		{
			cookie.MaxAge = (long)maxAge.TotalSeconds;
			return cookie;
		}
	}

	extension(IEnumerable<ClientCookie> cookies)
	{
		/// <summary>
		/// Converts cookies to Cookie header value.
		/// </summary>
		public string ToCookieHeaderValue() =>
			string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));
	}

	extension((string Name, string Value) pair)
	{
		/// <summary>
		/// Creates cookie from tuple.
		/// </summary>
		public ClientCookie ToCookie() => new(pair.Name, pair.Value);
	}
}