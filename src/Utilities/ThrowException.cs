using System;
using System.Collections.Generic;
using System.IO;

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
	}
}