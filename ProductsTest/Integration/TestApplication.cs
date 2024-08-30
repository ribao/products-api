using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace ProductsTest.Integration;

internal class TestApplication(string environment = "Development") : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(environment);
        return base.CreateHost(builder);
    }
}