using FluentAssertions;

using Http.TLS.Core;

using Xunit;

namespace Http.TLS.Unit.Test;

public class RequestClientFactoryTests
{
	[Fact]
	public void RequestClientFactory_CreateClient_1()
	{
		// Arrange
		var factory = new RequestClientFactory(o => o.BrowserType = BrowserType.Firefox132);

		// Act
		using var client = factory.CreateClient();

		// Assert
		client.Options.BrowserType.Should().Be(BrowserType.Firefox132);
	}

	[Fact]
	public void RequestClientFactory_CreateClient_2()
	{
		// Arrange
		var factory = new RequestClientFactory();
		factory.Register("firefox", o => o.BrowserType = BrowserType.Firefox132);

		// Act
		using var client = factory.CreateClient("firefox");

		// Assert
		client.Options.BrowserType.Should().Be(BrowserType.Firefox132);
	}

	[Fact]
	public void RequestClientFactory_CreateClient_3()
	{
		// Arrange
		var factory = new RequestClientFactory();

		// Act
		using var client = factory.CreateClient(o => o.WithDebug = true);

		// Assert
		client.Options.WithDebug.Should().BeTrue();
	}

	[Fact]
	public void RequestClientFactory_CreateClient_4()
	{
		// Arrange
		var factory = new RequestClientFactory(o =>
		{
			o.BrowserType = BrowserType.Chrome133;
			o.Timeout = TimeSpan.FromSeconds(10);
		});

		// Act
		using var client = factory.CreateClient(o =>
		{
			o.WithDebug = true;
			o.BrowserType = BrowserType.Chrome146;
		});

		// Assert
		client.Options.BrowserType.Should().Be(BrowserType.Chrome146);
		client.Options.Timeout.Should().Be(TimeSpan.FromSeconds(10));
		client.Options.WithDebug.Should().BeTrue();
	}

	[Fact]
	public void RequestClientFactory_CreateClient_5()
	{
		// Arrange
		var factory = new RequestClientFactory();
		var act = () => factory.CreateClient("unknown");

		// Act & Assert
		act.Should().Throw<KeyNotFoundException>().WithMessage("*'unknown'*");
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void RequestClientFactory_Register_1(string? name)
	{
		// Arrange
		var factory = new RequestClientFactory();
		var act = () => factory.Register(name!, _ => { });

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void RequestClientFactory_Register_2()
	{
		// Arrange
		var factory = new RequestClientFactory();
		var act = () => factory.Register("test", null!);

		// Act & Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void RequestClientFactory_Register_3()
	{
		// Arrange
		var factory = new RequestClientFactory();

		// Act & Assert
		factory.Register("a", _ => { }).Register("b", _ => { }).Should().BeSameAs(factory);
	}

	[Fact]
	public void RequestClientFactory_CreateClient_6()
	{
		// Arrange
		var factory = new RequestClientFactory(o => o.Timeout = TimeSpan.FromSeconds(5));
		factory.Register("c1", o => o.BrowserType = BrowserType.Chrome146);
		factory.Register("c2", o => o.BrowserType = BrowserType.Firefox132);

		// Act
		using var client1 = factory.CreateClient("c1");
		using var client2 = factory.CreateClient("c2");

		// Assert
		client1.Options.BrowserType.Should().Be(BrowserType.Chrome146);
		client2.Options.BrowserType.Should().Be(BrowserType.Firefox132);
		client1.Options.Timeout.Should().Be(client2.Options.Timeout);
	}
}