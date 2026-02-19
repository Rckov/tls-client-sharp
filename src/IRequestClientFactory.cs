using System;

namespace Http.TLS;

public interface IRequestClientFactory
{
	/// <summary>
	/// Creates a client using the default configuration.
	/// </summary>
	IRequestClient CreateClient();

	/// <summary>
	/// Creates a client using a named configuration registered via <see cref="RequestClientFactory.Register"/>.
	/// </summary>
	IRequestClient CreateClient(string name);

	/// <summary>
	/// Creates a client with inline configuration applied on top of the default.
	/// </summary>
	IRequestClient CreateClient(Action<RequestClientOptions> configure);
}