using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class ClientTests(NativeLibraryFixture nativeLibraryFixture)
{
	[Fact]
	public async Task Client_SessionId_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.Build();

		// Act
		var sessionId = client.SessionId;

		// Assert
		sessionId.Should().NotBeEmpty();
		client.Options.SessionId.Should().Be(sessionId);
	}

	[Fact]
	public async Task Client_Dispose_1()
	{
		// Arrange
		var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.Build();

		var sessionId = client.SessionId;

		// Act
		client.Dispose();

		// Assert
		sessionId.Should().NotBeEmpty();
		client.SessionId.Should().Be(sessionId);
	}

	[Fact]
	public async Task Client_BrowserType_1()
	{
		// Arrange
		using var chromeClient = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.Build();

		using var firefoxClient = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Firefox132)
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var chromeResponse = await chromeClient.SendAsync(request, TestContext.Current.CancellationToken);
		var firefoxResponse = await firefoxClient.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		chromeResponse.Should().NotBeNull();
		chromeResponse!.IsSuccessStatus.Should().BeTrue();
		firefoxResponse.Should().NotBeNull();
		firefoxResponse!.IsSuccessStatus.Should().BeTrue();
	}
}