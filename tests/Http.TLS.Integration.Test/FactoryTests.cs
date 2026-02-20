using FluentAssertions;

using Http.TLS.Core;
using Http.TLS.Extensions;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class FactoryTests(NativeLibraryFixture nativeLibraryFixture)
{
	[Fact]
	public async Task Factory_CreateClient_1()
	{
		// Arrange
		var factory = new RequestClientFactory(o => o.BrowserType = BrowserType.Chrome133);

		// Act
		using var client = factory.CreateClient();
		var response = await client.GetAsync("https://httpbin.org/get", TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
	}

	[Fact]
	public async Task Factory_NamedClient_1()
	{
		// Arrange
		var factory = new RequestClientFactory();
		factory.Register("chrome", o => o.BrowserType = BrowserType.Chrome133);

		// Act
		using var client = factory.CreateClient("chrome");
		var response = await client.GetAsync("https://httpbin.org/get", TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		client.Options.BrowserType.Should().Be(BrowserType.Chrome133);
	}

	[Fact]
	public async Task Factory_InlineConfiguration_1()
	{
		// Arrange
		var factory = new RequestClientFactory(o => o.Timeout = TimeSpan.FromSeconds(10));

		// Act
		using var client = factory.CreateClient(o => o.BrowserType = BrowserType.Chrome133);
		var response = await client.GetAsync("https://httpbin.org/get", TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		client.Options.Timeout.Should().Be(TimeSpan.FromSeconds(10));
		client.Options.BrowserType.Should().Be(BrowserType.Chrome133);
	}
}