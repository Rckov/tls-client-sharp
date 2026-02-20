using Http.TLS.Unit.Test.Builders;
using Http.TLS.Unit.Test.Utilities;

using System.Text.Json.Serialization;

namespace Http.TLS.Unit.Test;

[JsonSerializable(typeof(RequestBuilderTests.TestBody))]
[JsonSerializable(typeof(SerializerTests.BrowserPayload))]
internal partial class UnitTestJsonContext : JsonSerializerContext
{
}