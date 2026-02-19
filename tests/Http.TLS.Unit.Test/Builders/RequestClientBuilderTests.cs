using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Unit.Test.Builders;

public class RequestClientBuilderTests
{
	[Fact]
	public void WithUserAgent_ValidValue_AppearsInDefaultHeaders()
	{
		var builder = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithUserAgent("MyAgent/2.0");

		builder.Options.DefaultHeaders["User-Agent"].Should().ContainSingle("MyAgent/2.0");
	}

	[Fact]
	public void WithUserAgent_EmptyString_RemovesUserAgentHeader()
	{
		var builder = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithUserAgent("MyAgent/2.0")
			.WithUserAgent("");

		builder.Options.DefaultHeaders.Should().NotContainKey("User-Agent");
	}
}