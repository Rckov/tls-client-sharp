using Http.TLS.Native;

using Xunit;

namespace Http.TLS.Integration.Test.Fixtures;

public sealed class NativeLibraryFixture : IDisposable
{
	private readonly NativeClientContext _context = new("tls-client-windows-64-1.14.0.dll");

	public void Dispose() => _context.Dispose();
}

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<NativeLibraryFixture>;