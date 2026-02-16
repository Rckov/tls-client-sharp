using FluentAssertions;

using Http.TLS.Builders;
using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Test;

public class RequestTests
{
	private IRequestClient? client;

	public RequestTests()
	{
		RequestClient.Initialize("tls-client-windows-64-1.14.0.dll");
	}

	private RequestClient CreateClient()
	{
		return new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.Build();
	}

	[Fact]
	public void Send_GetRequest_ReturnsSuccessResponse()
	{
		client ??= CreateClient();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get")
			.WithMethod(HttpMethod.Get)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("httpbin");
	}

	[Fact]
	public void Send_GetWithHeaders_IncludesHeadersInRequest()
	{
		client ??= CreateClient();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/headers")
			.WithMethod(HttpMethod.Get)
			.WithHeader("X-Custom-Header", "test-value")
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("X-Custom-Header");
		response.Body.Should().Contain("test-value");
	}

	[Fact]
	public void Send_PostWithJson_SendsJsonBody()
	{
		client ??= CreateClient();

		var data = new { name = "test", value = 123 };

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/post")
			.WithMethod(HttpMethod.Post)
			.WithBody(data)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("test");
		response.Body.Should().Contain("123");
	}

	[Fact]
	public void Send_WithUserAgent_SetsUserAgent()
	{
		using var client = new RequestClientBuilder()
			.WithBrowserType(BrowserType.Chrome133)
			.WithTimeout(TimeSpan.FromSeconds(30))
			.WithUserAgent("CustomAgent/1.0")
			.Build();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/user-agent")
			.WithMethod(HttpMethod.Get)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("CustomAgent/1.0");
	}

	[Fact]
	public void Send_GetWithQueryParams_ReturnsParams()
	{
		client ??= CreateClient();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/get?param1=value1&param2=value2")
			.WithMethod(HttpMethod.Get)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("param1");
		response.Body.Should().Contain("value1");
	}

	[Fact]
	public void Send_PutRequest_UpdatesResource()
	{
		client ??= CreateClient();

		var data = new { title = "updated" };

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/put")
			.WithMethod(HttpMethod.Put)
			.WithBody(data)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("updated");
	}

	[Fact]
	public void Send_DeleteRequest_ReturnsSuccess()
	{
		client ??= CreateClient();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/delete")
			.WithMethod(HttpMethod.Delete)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
	}

	[Fact]
	public void Send_WithBasicAuth_AuthenticatesRequest()
	{
		client ??= CreateClient();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/basic-auth/user/pass")
			.WithMethod(HttpMethod.Get)
			.WithHeader("Authorization", "Basic dXNlcjpwYXNz")
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("authenticated");
	}

	[Fact]
	public void Send_WithMultipleHeaders_SendsAllHeaders()
	{
		client ??= CreateClient();

		var headers = new Dictionary<string, string>
		{
			["X-Header-1"] = "value1",
			["X-Header-2"] = "value2"
		};

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/headers")
			.WithMethod(HttpMethod.Get)
			.WithHeaders(headers)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(200);
		response.Body.Should().Contain("X-Header-1");
		response.Body.Should().Contain("X-Header-2");
	}

	[Fact]
	public void Send_GetStatus404_ReturnsNotFound()
	{
		client ??= CreateClient();

		var request = new RequestBuilder()
			.WithUrl("https://httpbin.org/status/404")
			.WithMethod(HttpMethod.Get)
			.Build();

		var response = client.Send(request);

		response.Should().NotBeNull();
		response!.Status.Should().Be(404);
	}
}