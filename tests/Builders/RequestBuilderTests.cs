using FluentAssertions;

using Http.TLS.Builders;

using Xunit;

namespace Http.TLS.Test.Builders;

public class RequestBuilderTests
{
	[Fact]
	public void Build_WithUrl_SetsRequestUrl()
	{
		var request = new RequestBuilder()
			.WithUrl("https://api.example.com")
			.Build();

		request.RequestUrl.Should().Be("https://api.example.com");
	}

	[Fact]
	public void Build_WithMethod_SetsHttpMethod()
	{
		var request = new RequestBuilder()
			.WithUrl("https://api.example.com")
			.WithMethod(HttpMethod.Post)
			.Build();

		request.RequestMethod.Should().Be("POST");
	}

	[Fact]
	public void Build_WithJsonBody_SerializesObject()
	{
		var data = new { name = "test", value = 123 };

		var request = new RequestBuilder()
			.WithUrl("https://api.example.com")
			.WithBody(data)
			.Build();

		request.RequestBody.Should().Contain("test");
		request.Headers["Content-Type"].Should().Be("application/json");
	}

	[Fact]
	public void Build_WithHeader_AddsHeader()
	{
		var request = new RequestBuilder()
			.WithUrl("https://api.example.com")
			.WithHeader("Authorization", "Bearer token")
			.Build();

		request.Headers["Authorization"].Should().Be("Bearer token");
	}
}