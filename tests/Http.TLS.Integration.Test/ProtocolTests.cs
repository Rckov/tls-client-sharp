using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class ProtocolTests(NativeLibraryFixture nativeLibraryFixture)
{
	[Fact]
	public async Task SendAsync_ForceHttp1_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithForceHttp1()
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.UsedProtocol.Should().MatchRegex("(?i)^http/1\\.1$");
	}

	[Fact]
	public async Task SendAsync_DisableHttp3_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithDisableHttp3()
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.UsedProtocol.Should().NotBe("h3");
	}

	[Fact]
	public async Task SendAsync_ProtocolRacing_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithProtocolRacing()
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.UsedProtocol.Should().NotBeNullOrEmpty();
	}
}