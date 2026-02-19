using FluentAssertions;

using Http.TLS.Core;
using Http.TLS.Extensions;

using Xunit;

namespace Http.TLS.Unit.Test.Extensions;

public class BrowserTypeExtensionsTests
{
	[Fact]
	public void Parse_ByDescription_ReturnsMappedEnum()
	{
		BrowserTypeExtensions.Parse("chrome_133").Should().Be(BrowserType.Chrome133);
	}

	[Fact]
	public void Parse_UnknownValue_Throws()
	{
		var act = () => BrowserTypeExtensions.Parse("unknown_browser_999");

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void TryParse_KnownValue_ReturnsTrueAndCorrectEnum()
	{
		var success = BrowserTypeExtensions.TryParse("chrome_133", out var result);

		success.Should().BeTrue();
		result.Should().Be(BrowserType.Chrome133);
	}

	[Fact]
	public void TryParse_UnknownValue_ReturnsFalse()
	{
		BrowserTypeExtensions.TryParse("unknown_999", out _).Should().BeFalse();
	}

	[Fact]
	public void ToDescription_ReturnsDescriptionAttributeValue()
	{
		BrowserType.Chrome133.ToDescription().Should().Be("chrome_133");
	}
}