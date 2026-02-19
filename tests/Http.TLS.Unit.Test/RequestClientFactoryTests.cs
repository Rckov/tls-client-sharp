using FluentAssertions;

using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Unit.Test;

public class RequestClientFactoryTests
{
	[Fact]
	public void CreateClient_WithDefaultConfigure_AppliesConfiguration()
	{
		var factory = new RequestClientFactory(o => o.BrowserType = BrowserType.Firefox132);

		using var client = factory.CreateClient();

		client.Options.BrowserType.Should().Be(BrowserType.Firefox132);
	}

	[Fact]
	public void CreateClient_ByRegisteredName_AppliesNamedConfiguration()
	{
		var factory = new RequestClientFactory();
		factory.Register("firefox", o => o.BrowserType = BrowserType.Firefox132);

		using var client = factory.CreateClient("firefox");

		client.Options.BrowserType.Should().Be(BrowserType.Firefox132);
	}

	[Fact]
	public void CreateClient_UnregisteredName_Throws()
	{
		var factory = new RequestClientFactory();

		var act = () => factory.CreateClient("nonexistent");

		act.Should().Throw<KeyNotFoundException>();
	}

	[Fact]
	public void CreateClient_WithInlineConfigure_AppliesConfiguration()
	{
		var factory = new RequestClientFactory();

		using var client = factory.CreateClient(o => o.BrowserType = BrowserType.Chrome133);

		client.Options.BrowserType.Should().Be(BrowserType.Chrome133);
	}
}