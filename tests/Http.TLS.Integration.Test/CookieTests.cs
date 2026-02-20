using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;
using Http.TLS.Integration.Test.Fixtures;

using Xunit;

namespace Http.TLS.Integration.Test;

[Collection("Integration Tests")]
[Trait("Category", "Integration")]
public class CookieTests(NativeLibraryFixture nativeLibraryFixture)
{
	private static IRequestClient CreateClient() =>
		new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();

	[Fact]
	public async Task SendAsync_SendCookies_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/cookies")
			.WithCookieJar()
			.WithCookie(new ClientCookie("session", "abc123"))
			.WithCookie(new ClientCookie("user_id", "12345"))
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Body.Should().Contain("session").And.Contain("abc123");
		response.Body.Should().Contain("user_id").And.Contain("12345");
	}

	[Fact]
	public async Task SendAsync_SetCookie_1()
	{
		// Arrange
		using var client = CreateClient();
		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/cookies/set?test_cookie=test_value")
			.WithFollowRedirects(true)
			.Build();

		// Act
		var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

		// Assert
		response.Should().NotBeNull();
		response!.IsSuccessStatus.Should().BeTrue();
		response.Cookies.Should().ContainKey("test_cookie");
	}

	[Fact]
	public async Task AddCookies_GetCookies_1()
	{
		// Arrange
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithCookieJar()
			.Build();

		var cookies = new List<ClientCookie>
		{
			new("session", "xyz789")
			{
				Domain = "httpbin.org",
				Path = "/",
				Secure = true,
				HttpOnly = true,
				MaxAge = 3600
			},
			new("token", "abc123")
			{
				Domain = "httpbin.org",
				Path = "/cookies"
			}
		};

		// Act
		var addResult = await client.AddCookiesAsync("https://httpbin.org", cookies, TestContext.Current.CancellationToken);
		var getResult = await client.GetCookiesAsync("https://httpbin.org", TestContext.Current.CancellationToken);

		// Assert
		addResult.Should().NotBeNull();
		getResult.Should().NotBeNull();

		if (getResult!.Cookies != null && getResult.Cookies.Count > 0)
		{
			getResult.Cookies.Should().HaveCountGreaterThanOrEqualTo(2);

			var sessionCookie = getResult.Cookies.FirstOrDefault(c => c.Name == "session");
			sessionCookie.Should().NotBeNull();
			sessionCookie!.Value.Should().Be("xyz789");
			sessionCookie.Domain.Should().Be("httpbin.org");
			sessionCookie.Path.Should().Be("/");
			sessionCookie.Secure.Should().BeTrue();
			sessionCookie.HttpOnly.Should().BeTrue();

			var tokenCookie = getResult.Cookies.FirstOrDefault(c => c.Name == "token");
			tokenCookie.Should().NotBeNull();
			tokenCookie!.Value.Should().Be("abc123");
		}
	}
}