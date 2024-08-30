using FluentAssertions;
using ProductsWebAPI.Handlers;
using ProductsWebAPI.Model;

namespace ProductsTest.Unit;

public class GetProductByIdHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_GivenValidId_ShouldReturnProduct()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = new Product { Name = "Test Product", Color = "Purple", Price = 9.99M };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var handler = new GetProductByIdHandler(dbContext);

        // Act
        var result = await handler.Handle(new GetProductByIdQuery(product.Id), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        result.Name.Should().Be("Test Product");
        result.Color.Should().Be("Purple");
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldReturnNull()
    {
        // Arrange
        await using var dbContext = CreateDbContext();

        var handler = new GetProductByIdHandler(dbContext);

        // Act
        var result = await handler.Handle(new GetProductByIdQuery(999), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
