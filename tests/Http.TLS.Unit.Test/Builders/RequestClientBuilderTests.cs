using FluentAssertions;

using Http.TLS.Builders;

using Xunit;

namespace Http.TLS.Unit.Test.Builders;

public class RequestClientBuilderTests
{
	[Fact]
	public void RequestClientBuilder_WithTimeout_1()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithTimeout(TimeSpan.Zero);

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*Timeout*");
	}

	[Fact]
	public void RequestClientBuilder_WithProxy_1()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithProxy("invalid-url");

		// Act & Assert
		act.Should().Throw<UriFormatException>();
	}

	[Fact]
	public void RequestClientBuilder_WithLocalAddress_1()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithLocalAddress("not-an-ip");

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*IP address*");
	}

	[Fact]
	public void RequestClientBuilder_WithCertificatePinning_2()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithCertificatePinning("valid.com", []);

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void RequestClientBuilder_Build_1()
	{
		// Arrange
		var builder = new RequestClientBuilder()
			.With(r => r.SessionId = Guid.Empty);
		var act = () => builder.Build();

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*SessionId*");
	}

	[Fact]
	public void RequestClientBuilder_Build_2()
	{
		// Arrange
		var builder = new RequestClientBuilder()
			.WithCookieJar()
			.WithoutCookieJar();
		var act = () => builder.Build();

		// Act & Assert
		act.Should().Throw<InvalidOperationException>().WithMessage("*CookieJar*");
	}

	[Fact]
	public void RequestClientBuilder_Build_3()
	{
		// Arrange
		var builder = new RequestClientBuilder()
			.WithDisableIPv4()
			.WithDisableIPv6();
		var act = () => builder.Build();

		// Act & Assert
		act.Should().Throw<InvalidOperationException>().WithMessage("*IPv4*IPv6*");
	}

	[Fact]
	public void RequestClientBuilder_Build_4()
	{
		// Arrange
		var builder = new RequestClientBuilder()
			.WithDefaultHeaders(new Dictionary<string, List<string>> { ["X"] = ["v"] });

		// Act
		var client1 = builder.Build();
		var client2 = builder.Build();

		// Assert
		client1.Options.DefaultHeaders.Should().NotBeSameAs(client2.Options.DefaultHeaders);
		client1.Options.DefaultHeaders["X"].Should().NotBeSameAs(client2.Options.DefaultHeaders["X"]);

		client1.Options.DefaultHeaders["X"].Add("v2");
		client2.Options.DefaultHeaders["X"].Should().HaveCount(1);
	}

	[Fact]
	public void RequestClientBuilder_WithTimeout_2()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithTimeout(TimeSpan.FromMinutes(31));

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*Timeout*");
	}

	[Fact]
	public void RequestClientBuilder_With_1()
	{
		// Arrange
		var customId = Guid.Parse("11111111-1111-1111-1111-111111111111");
		var builder = new RequestClientBuilder()
			.With(o =>
			{
				o.SessionId = customId;
				o.ForceHttp1 = true;
				o.DisableHttp3 = true;
			});

		// Act
		var options = builder.Options;

		// Assert
		options.SessionId.Should().Be(customId);
		options.ForceHttp1.Should().BeTrue();
		options.DisableHttp3.Should().BeTrue();
	}

	[Fact]
	public void RequestClientBuilder_WithCustomRequestClient_1()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithCustomRequestClient(null);

		// Act & Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void RequestClientBuilder_WithTransportOptions_1()
	{
		// Arrange
		var act = () => new RequestClientBuilder().WithTransportOptions(null);

		// Act & Assert
		act.Should().Throw<ArgumentNullException>();
	}
}