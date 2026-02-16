using FluentAssertions;

using Http.TLS.Core;
using Http.TLS.Utilities;

using Xunit;

namespace Http.TLS.Test.Utilities;

public class SerializerTests
{
	[Fact]
	public void Serialize_Object_ReturnsJson()
	{
		var data = new { name = "test", value = 123 };

		var json = Serializer.Serialize(data);

		json.Should().Contain("test");
		json.Should().Contain("123");
	}

	[Fact]
	public void Serialize_BrowserType_ReturnsDescription()
	{
		var data = new { browser = BrowserType.Chrome133 };

		var json = Serializer.Serialize(data);

		json.Should().Contain("chrome_133");
	}

	[Fact]
	public void Deserialize_Json_ReturnsObject()
	{
		const string json = """{"name":"test","value":123}""";

		var result = Serializer.Deserialize<TestData>(json);

		result!.Name.Should().Be("test");
		result.Value.Should().Be(123);
	}

	private class TestData
	{
		public string Name { get; set; } = "";
		public int Value { get; set; }
	}
}