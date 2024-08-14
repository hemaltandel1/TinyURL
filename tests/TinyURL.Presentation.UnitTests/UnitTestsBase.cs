using Microsoft.Extensions.DependencyInjection;

namespace TinyURL.Presentation.UnitTests;

public abstract class UnitTestsBase
{
    public readonly ServiceProvider _serviceProvider;

    protected UnitTestsBase()
    {
        _serviceProvider = Program.ConfigureServices();
    }
}
