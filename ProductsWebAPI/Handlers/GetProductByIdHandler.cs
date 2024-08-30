using MediatR;
using ProductsWebAPI.Data;
using ProductsWebAPI.Model;

namespace ProductsWebAPI.Handlers;

public class GetProductByIdHandler(ProductsDbContext dbContext) : IRequestHandler<GetProductByIdQuery, Product?>
{
    public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Products.FindAsync([request.Id], cancellationToken);
    }
}