using MediatR;

namespace ProductsWebAPI.Model;

public record GetProductsByColorQuery(string Color) : IRequest<List<Product>>;