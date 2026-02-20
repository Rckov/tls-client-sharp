using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using System.Text;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class BodyTests(NativeLibraryFixture nativeLibraryFixture)
{
	private static IRequestClient CreateClient() =>
		new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

	[Fact]
	public async Task SendAsync_JsonBody_1()
	{
		// Arrange
		using var client = CreateClient();
		var payload = new { name = "John", age = 30, active = true };
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(payload)
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("John").And.Contain("30").And.Contain("true");
	}

	[Fact]
	public async Task SendAsync_StringBody_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody("plain text content")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("plain text content");
	}

	[Fact]
	public async Task SendAsync_BinaryBody_1()
	{
		// Arrange
		using var client = CreateClient();
		var bytes = Encoding.UTF8.GetBytes("binary data test");
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(bytes)
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("binary data test");
	}

	[Fact]
	public async Task SendAsync_EmptyBody_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
	}
}