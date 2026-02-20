using Http.TLS.Native;
using Http.TLS.Unit.Test;

using Xunit;

[assembly: AssemblyFixture(typeof(TestSetup))]

namespace Http.TLS.Unit.Test;

public sealed class TestSetup
{
	public TestSetup()
	{
		NativeClientContext.RegisteredContexts.Add(UnitTestJsonContext.Default);
	}
}