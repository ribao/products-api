using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsWebAPI.Data;
using ProductsWebAPI.Model;

namespace ProductsWebAPI.Handlers;

public class GetProductsByColorHandler(ProductsDbContext dbContext)
    : IRequestHandler<GetProductsByColorQuery, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsByColorQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Products.Where(p => p.Color == request.Color).ToListAsync(cancellationToken);
    }
}