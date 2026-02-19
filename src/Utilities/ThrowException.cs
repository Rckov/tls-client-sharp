using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace Http.TLS.Utilities;

internal static class ThrowException
{
    extension<T>(T? value) where T : class
    {
        public T ThrowIfNull([CallerArgumentExpression(nameof(value))] string? paramName = null) =>
            value ?? throw new ArgumentNullException(paramName);
    }

    extension<T>(ICollection<T>? value)
    {
        public ICollection<T> ThrowIfEmpty([CallerArgumentExpression(nameof(value))] string? paramName = null) =>
            value is { Count: > 0 }
                ? value
                : throw new ArgumentException("Collection cannot be empty.", paramName);
    }

    extension(string? value)
    {
        public string ThrowIfNullOrEmpty([CallerArgumentExpression(nameof(value))] string? paramName = null) =>
            !string.IsNullOrWhiteSpace(value)
                ? value!
                : throw new ArgumentException("Value cannot be null or empty.", paramName);

        public string ThrowIfNotUri([CallerArgumentExpression(nameof(value))] string? paramName = null) =>
            Uri.TryCreate(value, UriKind.Absolute, out _)
                ? value!
                : throw new UriFormatException($"'{value}' is not a valid absolute URI.");

        public string ThrowIfNotFileExists([CallerArgumentExpression(nameof(value))] string? paramName = null) =>
            File.Exists(value)
                ? value!
                : throw new FileNotFoundException($"File not found at path: '{value}'", value);

        public string ThrowIfInvalidHostname([CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            value.ThrowIfNullOrEmpty(paramName);
            return Uri.CheckHostName(value) != UriHostNameType.Unknown
                ? value!
                : throw new ArgumentException($"Invalid hostname: '{value}'.", paramName);
        }

        public string ThrowIfInvalidIpAddress([CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            value.ThrowIfNullOrEmpty(paramName);
            return IPAddress.TryParse(value, out _)
                ? value!
                : throw new ArgumentException("Value must be a valid IP address.", paramName);
        }
    }

    extension(TimeSpan value)
    {
        public TimeSpan ThrowIfInvalidTimeout([CallerArgumentExpression(nameof(value))] string? paramName = null) =>
            value > TimeSpan.Zero && value <= TimeSpan.FromMinutes(30)
                ? value
                : throw new ArgumentException("Timeout must be between 1ms and 30 minutes.", paramName);
    }
}
