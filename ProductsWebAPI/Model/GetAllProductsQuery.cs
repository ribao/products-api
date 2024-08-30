using MediatR;

namespace ProductsWebAPI.Model;

public record GetAllProductsQuery() : IRequest<List<Product>>;