using ProductsWebAPI.Handlers;
using ProductsWebAPI.Model;

namespace ProductsTest.Unit;

using Xunit;
using FluentAssertions;

public class CreateProductHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCreateProduct()
    {
        // Arrange
        await using var dbContext = CreateDbContext();

        var handler = new CreateProductHandler(dbContext);
        var request = new CreateProductCommand("Test Product", "Purple", 9.99M);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Product");
        result.Color.Should().Be("Purple");
        result.Price.Should().Be(9.99M);

        var productInDb = await dbContext.Products.FindAsync(result.Id);
        productInDb.Should().NotBeNull();
        productInDb.Name.Should().Be("Test Product");
        productInDb.Color.Should().Be("Purple");
        productInDb.Price.Should().Be(9.99M);
    }
}
