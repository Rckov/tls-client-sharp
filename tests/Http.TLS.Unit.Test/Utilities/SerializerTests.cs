using FluentAssertions;

using Http.TLS.Core;
using Http.TLS.Utilities;

using Xunit;

namespace Http.TLS.Unit.Test.Utilities;

public class SerializerTests
{
	public sealed record TestResponse(int Status, string? Body);

	[Fact]
	public void BrowserType_Serialize_1()
	{
		// Arrange

		// Act
		var json = Serializer.Serialize(BrowserType.Chrome133);

		// Assert
		json.Should().Be("\"chrome_133\"");
	}

	[Fact]
	public void BrowserType_Deserialize_2()
	{
		// Arrange
		var type = BrowserType.Chrome133Psk;

		// Act
		var json = Serializer.Serialize(type);
		var result = Serializer.Deserialize<BrowserType>(json);

		// Assert
		result!.Value.Should().Be(type.Value);
	}

	[Fact]
	public void BrowserType_Serialize_3()
	{
		// Arrange
		var act = () => Serializer.Serialize<BrowserType>(null);

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void TestResponse_Serialize_4()
	{
		// Arrange
		var payload = new TestResponse(200, null);

		// Act
		var json = Serializer.Serialize(payload);

		// Assert
		json.Should().Contain("\"status\":200").And.NotContain("body");
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void TestResponse_Deserialize_5(string? input)
	{
		// Arrange
		var act = () => Serializer.Deserialize<TestResponse>(input);

		// Act & Assert
		act.Should().Throw<ArgumentException>();
	}
}