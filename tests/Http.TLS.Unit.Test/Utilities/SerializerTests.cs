using FluentAssertions;

using Http.TLS.Core;
using Http.TLS.Core.Response;
using Http.TLS.Utilities;

using Xunit;

namespace Http.TLS.Unit.Test.Utilities;

public class SerializerTests
{
	[Fact]
	public void Serialize_BrowserType_UsesDescriptionNotEnumName()
	{
		var json = Serializer.Serialize(new { browser = BrowserType.Chrome133 });

		json.Should().Contain("chrome_133").And.NotContain("Chrome133");
	}

	[Fact]
	public void Deserialize_ValidJson_ReturnsObject()
	{
		var result = Serializer.Deserialize<Response>("{\"status\":200,\"body\":\"ok\"}");

		result!.Status.Should().Be(200);
		result.Body.Should().Be("ok");
	}
}