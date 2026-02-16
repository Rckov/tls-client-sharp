using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Test.Builders;

public class RequestClientBuilderTests
{
	[Fact]
	public void Build_WithBrowserAndTimeout_CreatesClient()
	{
		var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

		client.Options.BrowserType.Should().Be(BrowserType.Chrome133);
		client.Options.Timeout.Should().Be(TimeSpan.FromSeconds(30));
	}

	[Fact]
	public void Build_WithProxy_SetsProxyUrl()
	{
		var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Firefox132)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.WithProxy("http://proxy:8080")
			.Build();

		client.Options.ProxyUrl.Should().Be("http://proxy:8080");
	}

	[Fact]
	public void Build_WithCookieJar_EnablesCookies()
	{
		var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.WithCookieJar()
			.Build();

		client.Options.WithCustomCookieJar.Should().BeTrue();
	}

	[Fact]
	public void Build_WithDebug_EnablesDebugMode()
	{
		var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.WithDebug()
			.Build();

		client.Options.WithDebug.Should().BeTrue();
	}
}