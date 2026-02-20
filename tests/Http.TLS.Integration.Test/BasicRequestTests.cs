using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class BasicRequestTests(NativeLibraryFixture nativeLibraryFixture)
{
	private static IRequestClient CreateClient() =>
		new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

	[Fact]
	public async Task SendAsync_Get_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Status.Should().Be(200);
		response.Body.Should().NotBeNullOrEmpty();
		response.UsedProtocol.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task SendAsync_Post_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(new { key = "test_value", number = 42 })
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("test_value").And.Contain("42");
	}

	[Fact]
	public async Task SendAsync_Put_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/put")
			.WithMethod(HttpMethod.Put)
			.WithBody(new { updated = true })
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("updated");
	}

	[Fact]
	public async Task SendAsync_Delete_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/delete")
			.WithMethod(HttpMethod.Delete)
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
	}

	[Fact]
	public async Task SendAsync_Status404_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/status/404")
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.Status.Should().Be(404);
		response.IsSuccessStatus.Should().BeFalse();
	}

	[Fact]
	public async Task SendAsync_Timeout_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/delay/10")
			.WithTimeout(TimeSpan.FromMilliseconds(100))
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.Status.Should().Be(0);
	}

	[Fact]
	public void Send_Get_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		// Act
		var response = client.Send(request);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Status.Should().Be(200);
	}

	[Fact]
	public async Task SendAsync_Redirect_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/redirect/2")
			.WithFollowRedirects(true)
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Target.Should().Contain("/get");
	}
}