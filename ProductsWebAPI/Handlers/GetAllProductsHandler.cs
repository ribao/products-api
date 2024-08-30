using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsWebAPI.Data;
using ProductsWebAPI.Model;

namespace ProductsWebAPI.Handlers;

public class GetAllProductsHandler(ProductsDbContext dbContext) : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Products.ToListAsync(cancellationToken);
    }
}