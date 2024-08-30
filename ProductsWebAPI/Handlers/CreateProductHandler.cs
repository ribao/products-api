using ProductsWebAPI.Data;
using ProductsWebAPI.Model;

namespace ProductsWebAPI.Handlers;

using MediatR;

public class CreateProductHandler(ProductsDbContext dbContext) : IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Color = request.Color,
            Price = request.Price
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }
}