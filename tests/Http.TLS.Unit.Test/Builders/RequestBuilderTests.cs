using FluentAssertions;

using Http.TLS.Builders;

using System.Text;

using Xunit;

namespace Http.TLS.Unit.Test.Builders;

public class RequestBuilderTests
{
	[Fact]
	public void RequestBuilder_Build_1()
	{
		// Arrange
		var builder = new RequestBuilder();

		// Act
		var request = builder.Build();

		// Assert
		request.RequestMethod.Should().Be("GET");
		request.RequestUrl.Should().BeEmpty();
		request.Headers.Should().BeEmpty();
		request.RequestCookies.Should().BeEmpty();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("not-a-uri")]
	public void RequestBuilder_WithUrl_1(string? url)
	{
		// Arrange
		var act = () => new RequestBuilder().WithUrl(url!);

		// Act & Assert
		act.Should().Throw<UriFormatException>();
	}

	[Fact]
	public void RequestBuilder_WithTimeout_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithTimeout(TimeSpan.Zero);

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void RequestBuilder_WithBody_1()
	{
		// Arrange
		var payload = new { name = "test", value = 42 };

		// Act
		var request = new RequestBuilder()
			.WithBody(payload)
			.Build();

		// Assert
		request.RequestBody.Should().Contain("\"name\":\"test\"");
		request.Headers["Content-Type"].Should().Be("application/json");
	}

	[Fact]
	public void RequestBuilder_WithBody_2()
	{
		// Arrange
		var bytes = new byte[51 * 1024 * 1024];
		var act = () => new RequestBuilder().WithBody(bytes);

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*too large*");
	}

	[Fact]
	public void RequestBuilder_WithBody_3()
	{
		// Arrange
		var bytes = Encoding.UTF8.GetBytes("binary");

		// Act
		var request = new RequestBuilder().WithBody(bytes).Build();

		// Assert
		request.RequestBody.Should().Be("YmluYXJ5");
		request.IsByteRequest.Should().BeTrue();
	}

	[Fact]
	public void RequestBuilder_WithCertificatePinning_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithCertificatePinning("valid.com", []);

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void RequestBuilder_WithStreamOutput_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithStreamOutput("path", size: 0);

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*positive*");
	}

	[Fact]
	public void RequestBuilder_WithHeaders_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithHeaders(null!);

		// Act & Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void RequestBuilder_WithHeaderOrder_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithHeaderOrder(null!);

		// Act & Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void RequestBuilder_With_1()
	{
		// Arrange
		var expected = Guid.Parse("11111111-1111-1111-1111-111111111111");

		// Act
		var request = new RequestBuilder()
			.With(r => r.SessionId = expected)
			.Build();

		// Assert
		request.SessionId.Should().Be(expected);
	}

	[Fact]
	public void RequestBuilder_Build_2()
	{
		// Arrange
		var builder = new RequestBuilder()
			.WithUrl("https://api.test/endpoint  ")
			.WithMethod(HttpMethod.Put)
			.WithTimeout(TimeSpan.FromMinutes(2))
			.WithHeader("Authorization", "Bearer xyz")
			.WithBody("{\"key\":\"value\"}")
			.WithInsecureSkipVerify()
			.WithForceHttp1();

		// Act
		var request = builder.Build();

		// Assert
		request.RequestUrl.Should().Be("https://api.test/endpoint  ");
		request.RequestMethod.Should().Be("PUT");
		request.TimeoutMilliseconds.Should().Be(120_000);
		request.Headers["Authorization"].Should().Be("Bearer xyz");
		request.RequestBody.Should().Be("{\"key\":\"value\"}");
		request.InsecureSkipVerify.Should().BeTrue();
		request.ForceHttp1.Should().BeTrue();
	}

	[Fact]
	public void RequestBuilder_WithProxy_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithProxy("invalid-url");

		// Act & Assert
		act.Should().Throw<UriFormatException>();
	}

	[Fact]
	public void RequestBuilder_WithLocalAddress_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithLocalAddress("not-an-ip");

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*IP address*");
	}

	[Fact]
	public void RequestBuilder_WithServerName_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithServerName("invalid hostname!");

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*hostname*");
	}

	[Fact]
	public void RequestBuilder_WithHostOverride_1()
	{
		// Arrange
		var act = () => new RequestBuilder().WithHostOverride("invalid hostname!");

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*hostname*");
	}
}