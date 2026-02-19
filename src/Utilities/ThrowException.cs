using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Http.TLS.Utilities;

internal static class ThrowException
{
	extension<T>(T? value) where T : class
	{
		public T ThrowIfNull(string? paramName = null)
		{
			return value ?? throw new ArgumentNullException(paramName);
		}

		public T Pipe(Action<T>? validate, string? paramName = null)
		{
			if (value is null)
			{
				throw new ArgumentNullException(paramName);
			}

			validate?.Invoke(value);
			return value;
		}
	}

	extension<T>(ICollection<T>? value)
	{
		public ICollection<T> ThrowIfEmpty(string? paramName = "value")
		{
			if (value is null || value.Count == 0)
			{
				throw new ArgumentException("Collection cannot be empty.", paramName);
			}

			return value;
		}
	}

	extension(string? value)
	{
		public string IsUri()
		{
			return Uri.TryCreate(value, UriKind.Absolute, out _)
				? value!
				: throw new UriFormatException();
		}

		public string ThrowIfNullOrEmpty(string? paramName = null)
		{
			return !string.IsNullOrWhiteSpace(value)
				? value!
				: throw new ArgumentException("Value cannot be null or empty.", paramName);
		}

		public string ThrowIfNotFileExists()
		{
			return File.Exists(value)
				? value!
				: throw new FileNotFoundException($"File not found at path: '{value}'", value);
		}

		public string ThrowIfInvalidHostname(string? paramName = null)
		{
			value.ThrowIfNullOrEmpty(paramName);
			return Uri.CheckHostName(value) != UriHostNameType.Unknown
				? value!
				: throw new ArgumentException($"Invalid hostname: '{value}'.", paramName);
		}

		public string ThrowIfInvalidIpAddress(string? paramName = null)
		{
			value.ThrowIfNullOrEmpty(paramName);
			return IPAddress.TryParse(value, out _)
				? value!
				: throw new ArgumentException("Value must be a valid IP address.", paramName);
		}
	}

	extension(TimeSpan value)
	{
		public TimeSpan ThrowIfInvalidTimeout(string? paramName = null)
		{
			return value > TimeSpan.Zero && value <= TimeSpan.FromMinutes(30)
				? value
				: throw new ArgumentException("Timeout must be between 1ms and 30 minutes.", paramName);
		}
	}
}