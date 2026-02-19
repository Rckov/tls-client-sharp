using FluentAssertions;

using Http.TLS.Builders;

using Xunit;

namespace Http.TLS.Unit.Test.Builders;

public class RequestBuilderTests
{
	[Fact]
	public void WithUrl_InvalidUri_Throws()
	{
		var act = () => new RequestBuilder().WithUrl("not-a-uri");

		act.Should().Throw<UriFormatException>();
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void WithTimeout_ZeroOrNegative_Throws(int seconds)
	{
		var act = () => new RequestBuilder().WithTimeout(TimeSpan.FromSeconds(seconds));

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void WithTimeout_ExceedsThirtyMinutes_Throws()
	{
		var act = () => new RequestBuilder().WithTimeout(TimeSpan.FromMinutes(31));

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void WithBody_Object_SerializesAndSetsContentType()
	{
		var request = new RequestBuilder()
			.WithUrl("https://example.com")
			.WithBody(new { name = "test", value = 42 })
			.Build();

		request.RequestBody.Should().Be("{\"name\":\"test\",\"value\":42}");
		request.Headers["Content-Type"].Should().Be("application/json");
	}

	[Fact]
	public void WithBody_BytesExceedLimit_Throws()
	{
		var bytes = new byte[(50 * 1024 * 1024) + 1];

		var act = () => new RequestBuilder().WithBody(bytes);

		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void WithBody_ValidBytes_SetsBase64AndByteRequestFlag()
	{
		var bytes = new byte[] { 1, 2, 3 };

		var request = new RequestBuilder()
			.WithUrl("https://example.com")
			.WithBody(bytes)
			.Build();

		request.IsByteRequest.Should().BeTrue();
		request.RequestBody.Should().Be(Convert.ToBase64String(bytes));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void WithStreamOutput_NonPositiveBlockSize_Throws(int size)
	{
		var act = () => new RequestBuilder().WithStreamOutput("/tmp/out", size: size);

		act.Should().Throw<ArgumentException>();
	}
}