using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Native;

using Xunit;

namespace Http.TLS.Integration.Test;

public sealed class NativeLibraryFixture : IDisposable
{
	private readonly NativeClientContext _context = new("tls-client-windows-64-1.14.0.dll");

	public NativeLibraryFixture()
	{
		_context.RegisterContext(IntegrationTestJsonContext.Default);
	}

	public void Dispose() => _context.Dispose();
}

[Trait("Category", "Integration")]
public class RequestTests(NativeLibraryFixture nativeLibraryFixture) : IClassFixture<NativeLibraryFixture>
{
	internal sealed record PostBody(string Key);

	private static IRequestClient CreateClient() =>
		new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

	[Fact]
	public async Task SendAsync_Get_ReturnsSuccessResponse()
	{
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.Build();

		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task SendAsync_PostJson_BodyReflectedInResponse()
	{
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(new PostBody("sentinel_value"))
			.Build();

		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("sentinel_value");
	}
}