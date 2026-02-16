using Http.TLS.Core;
using Http.TLS.Utilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Http.TLS.Extensions;

/// <summary>
/// BrowserType string conversion extensions.
/// </summary>
public static class BrowserTypeExtensions
{
	private static readonly Lazy<Dictionary<string, BrowserType>> Mapping = new(BuildMapping);

	/// <summary>
	/// Gets description attribute value.
	/// </summary>
	public static string ToDescription(this BrowserType type)
	{
		return typeof(BrowserType)
			.GetMember(type.ToString())[0]
			.GetCustomAttribute<DescriptionAttribute>()?
			.Description ?? type.ToString();
	}

	/// <summary>
	/// Parses string to BrowserType.
	/// </summary>
	public static BrowserType Parse(string? value)
	{
		value.ThrowIfNullOrEmpty(nameof(value));

		if (Mapping.Value.TryGetValue(value!, out var result))
		{
			return result;
		}

		throw new ArgumentException($"Unknown browser type: '{value}'", nameof(value));
	}

	/// <summary>
	/// Tries to parse string to BrowserType.
	/// </summary>
	public static bool TryParse(string? value, out BrowserType result)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			result = default;
			return false;
		}

		return Mapping.Value.TryGetValue(value!, out result);
	}

	private static Dictionary<string, BrowserType> BuildMapping()
	{
		var mapping = new Dictionary<string, BrowserType>(StringComparer.OrdinalIgnoreCase);

		foreach (BrowserType type in Enum.GetValues(typeof(BrowserType)))
		{
			var description = type.ToDescription();
			mapping[description] = type;
			mapping[type.ToString()] = type;
		}

		return mapping;
	}
}