using FluentAssertions;
using ProductsWebAPI.Handlers;
using ProductsWebAPI.Model;

namespace ProductsTest.Unit;

public class GetProductsByColorHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_GivenValidColor_ShouldReturnMatchingProducts()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.AddRange(new List<Product>
        {
            new() { Name = "Red iPhone", Color = "Red", Price = 1000.00M },
            new() { Name = "Blue iPhone", Color = "Blue", Price = 1500.00M },
            new() { Name = "Another Red iPhone", Color = "Red", Price = 2000.00M }
        });
        await dbContext.SaveChangesAsync();

        var handler = new GetProductsByColorHandler(dbContext);

        // Act
        var result = await handler.Handle(new GetProductsByColorQuery("Red"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.Color == "Red");
    }
}
