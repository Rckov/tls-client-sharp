using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class ResponseTests(NativeLibraryFixture nativeLibraryFixture)
{
	private static IRequestClient CreateClient() =>
		new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

	[Fact]
	public async Task Response_GetHeader_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/response-headers?X-Test=value")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.GetHeader("X-Test").Should().Be("value");
		response.GetHeader("NonExistent").Should().BeNull();
	}

	[Fact]
	public async Task Response_GetCookie_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/cookies/set?test=value123")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.HasCookie("test").Should().BeTrue();
		response.GetCookie("test").Should().Be("value123");
		response.GetCookie("nonexistent").Should().BeNull();
	}
}