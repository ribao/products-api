using MediatR;

namespace ProductsWebAPI.Model;

public record GetProductByIdQuery(int Id) : IRequest<Product?>;