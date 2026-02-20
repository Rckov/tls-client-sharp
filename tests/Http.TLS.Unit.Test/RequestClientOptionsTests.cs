using FluentAssertions;

using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Unit.Test;

public class RequestClientOptionsTests
{
	[Fact]
	public void RequestClientOptions_Validate_1()
	{
		// Arrange
		var options = new RequestClientOptions
		{
			DisableIPv4 = true,
			DisableIPv6 = true
		};
		var act = () => options.Validate();

		// Act & Assert
		act.Should().Throw<InvalidOperationException>().WithMessage("*IPv4*IPv6*");
	}

	[Fact]
	public void RequestClientOptions_Validate_2()
	{
		// Arrange
		var options = new RequestClientOptions
		{
			ProxyUrl = "not-a-valid-url"
		};
		var act = () => options.Validate();

		// Act & Assert
		act.Should().Throw<UriFormatException>();
	}

	[Fact]
	public void RequestClientOptions_Validate_3()
	{
		// Arrange
		var options = new RequestClientOptions
		{
			LocalAddress = "invalid-ip"
		};
		var act = () => options.Validate();

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*IP address*");
	}

	[Fact]
	public void RequestClientOptions_Validate_4()
	{
		// Arrange
		var options = new RequestClientOptions();
		options.CertificatePinningHosts["example.com"] = [];
		var act = () => options.Validate();

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void RequestClientOptions_Validate_5()
	{
		// Arrange
		var options = new RequestClientOptions();
		options.CertificatePinningHosts["invalid hostname!"] = ["pin1"];
		var act = () => options.Validate();

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*hostname*");
	}

	[Fact]
	public void RequestClientOptions_Validate_6()
	{
		// Arrange
		var options = new RequestClientOptions
		{
			Timeout = TimeSpan.Zero
		};
		var act = () => options.Validate();

		// Act & Assert
		act.Should().Throw<ArgumentException>().WithMessage("*Timeout*positive*");
	}

	[Fact]
	public void RequestClientOptions_Clone_1()
	{
		// Arrange
		var original = new RequestClientOptions();
		original.DefaultHeaders["X-Custom"] = ["value1", "value2"];
		original.CertificatePinningHosts["example.com"] = ["pin1", "pin2"];
		original.HeaderOrder.Add("Authorization");

		// Act
		var clone = original.Clone();

		// Assert
		clone.DefaultHeaders.Should().NotBeSameAs(original.DefaultHeaders);
		clone.DefaultHeaders["X-Custom"].Should().NotBeSameAs(original.DefaultHeaders["X-Custom"]);
		clone.CertificatePinningHosts.Should().NotBeSameAs(original.CertificatePinningHosts);
		clone.CertificatePinningHosts["example.com"].Should().NotBeSameAs(original.CertificatePinningHosts["example.com"]);
		clone.HeaderOrder.Should().NotBeSameAs(original.HeaderOrder);

		original.DefaultHeaders["X-Custom"].Add("value3");
		clone.DefaultHeaders["X-Custom"].Should().HaveCount(2);

		original.CertificatePinningHosts["example.com"].Add("pin3");
		clone.CertificatePinningHosts["example.com"].Should().HaveCount(2);

		original.HeaderOrder.Add("Content-Type");
		clone.HeaderOrder.Should().HaveCount(1);
	}

	[Fact]
	public void RequestClientOptions_Clone_2()
	{
		// Arrange
		var original = new RequestClientOptions
		{
			CustomRequestClient = new CustomRequestClient
			{
				Ja3Fingerprint = "test",
				AlpnProtocols = ["h2", "http/1.1"]
			},
			TransportOptions = new TransportOptions
			{
				MaxIdleConns = 100
			}
		};

		// Act
		var clone = original.Clone();

		// Assert
		clone.CustomRequestClient.Should().NotBeSameAs(original.CustomRequestClient);
		clone.TransportOptions.Should().NotBeSameAs(original.TransportOptions);

		original.CustomRequestClient.AlpnProtocols.Add("h3");
		clone.CustomRequestClient!.AlpnProtocols.Should().HaveCount(2);
	}
}