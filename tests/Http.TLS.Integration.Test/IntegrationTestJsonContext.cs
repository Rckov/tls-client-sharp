using System.Text.Json.Serialization;

namespace Http.TLS.Integration.Test;

[JsonSerializable(typeof(RequestTests.PostBody))]
internal partial class IntegrationTestJsonContext : JsonSerializerContext
{
}