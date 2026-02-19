using FluentAssertions;

using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Unit.Test;

public class RequestClientOptionsTests
{
	[Fact]
	public void Clone_MutatingCollections_DoesNotAffectOriginal()
	{
		var original = new RequestClientOptions
		{
			DefaultHeaders = new() { ["X-Test"] = ["value"] },
			HeaderOrder = ["Accept"],
			CertificatePinningHosts = new() { ["example.com"] = ["abc123"] },
			CustomRequestClient = new CustomRequestClient { AlpnProtocols = ["h2"] }
		};

		var clone = original.Clone();

		clone.DefaultHeaders["X-Test"].Add("extra");
		clone.HeaderOrder.Add("Content-Type");
		clone.CertificatePinningHosts["example.com"].Add("def456");
		clone.CustomRequestClient!.AlpnProtocols.Add("http/1.1");

		original.DefaultHeaders["X-Test"].Should().HaveCount(1);
		original.HeaderOrder.Should().HaveCount(1);
		original.CertificatePinningHosts["example.com"].Should().HaveCount(1);
		original.CustomRequestClient!.AlpnProtocols.Should().HaveCount(1);
	}

	[Fact]
	public void Validate_BothCookieJarFlags_Throws()
	{
		var options = new RequestClientOptions
		{
			WithCustomCookieJar = true,
			WithoutCookieJar = true
		};

		options.Invoking(o => o.Validate()).Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void Validate_BothIpVersionsDisabled_Throws()
	{
		var options = new RequestClientOptions
		{
			DisableIPv4 = true,
			DisableIPv6 = true
		};

		options.Invoking(o => o.Validate()).Should().Throw<InvalidOperationException>();
	}
}