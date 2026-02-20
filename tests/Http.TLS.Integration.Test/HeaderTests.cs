using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class HeaderTests(NativeLibraryFixture nativeLibraryFixture)
{
	private static IRequestClient CreateClient() =>
		new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

	[Fact]
	public async Task SendAsync_CustomHeaders_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/headers")
			.WithHeader("X-Custom-Header", "test-value")
			.WithHeader("X-Another-Header", "another-value")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("X-Custom-Header").And.Contain("test-value");
		response.Body.Should().Contain("X-Another-Header").And.Contain("another-value");
	}

	[Fact]
	public async Task SendAsync_UserAgent_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithUserAgent("CustomAgent/1.0")
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/user-agent")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("CustomAgent/1.0");
	}

	[Fact]
	public async Task SendAsync_ResponseHeaders_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/response-headers?X-Test=value123")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.GetHeader("X-Test").Should().Be("value123");
	}

	[Fact]
	public async Task SendAsync_ContentType_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(new { data = "test" })
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("application/json");
	}
}