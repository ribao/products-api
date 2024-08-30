using Microsoft.EntityFrameworkCore;
using ProductsWebAPI.Data;

namespace ProductsTest.Unit;

public abstract class HandlerTestBase
{
    protected ProductsDbContext CreateDbContext() =>
        new(new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options);
}
