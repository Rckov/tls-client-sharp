using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class TransportTests(NativeLibraryFixture nativeLibraryFixture)
{
	[Fact]
	public async Task SendAsync_TransportOptions_DisableKeepAlives_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTransportOptions(new TransportOptions
			{
				DisableKeepAlives = true
			})
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
	}

	[Fact]
	public async Task SendAsync_TransportOptions_DisableCompression_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTransportOptions(new TransportOptions
			{
				DisableCompression = true
			})
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/gzip")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
	}

	[Fact]
	public async Task SendAsync_InsecureSkipVerify_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithInsecureSkipVerify()
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
	}
}