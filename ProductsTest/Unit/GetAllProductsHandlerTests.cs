using FluentAssertions;
using ProductsWebAPI.Handlers;
using ProductsWebAPI.Model;

namespace ProductsTest.Unit;

public class GetAllProductsHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnAllProducts()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.AddRange(new List<Product>
        {
            new() { Name = "Product 1", Color = "Red", Price = 10.00M },
            new() { Name = "Product 2", Color = "Blue", Price = 15.00M }
        });
        await dbContext.SaveChangesAsync();

        var handler = new GetAllProductsHandler(dbContext);

        // Act
        var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
}
