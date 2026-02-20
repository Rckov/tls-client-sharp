#if NET8_0_OR_GREATER

using Http.TLS.Core;
using Http.TLS.Core.Request;
using Http.TLS.Core.Response;

using System.Text.Json.Serialization;

namespace Http.TLS.Utilities;

[JsonSerializable(typeof(Request))]
[JsonSerializable(typeof(Response))]
[JsonSerializable(typeof(AddCookiesRequest))]
[JsonSerializable(typeof(GetCookiesRequest))]
[JsonSerializable(typeof(CookiesResponse))]
[JsonSerializable(typeof(ClientCookie))]
[JsonSerializable(typeof(CustomRequestClient))]
[JsonSerializable(typeof(TransportOptions))]
[JsonSerializable(typeof(CandidateCipherSuite))]
[JsonSerializable(typeof(PriorityFrame))]
[JsonSerializable(typeof(PriorityParam))]
[JsonSerializable(typeof(BrowserType))]
[JsonSerializable(typeof(DestroySessionRequest))]
internal partial class SerializerContext : JsonSerializerContext;

#endif
