using MediatR;

namespace ProductsWebAPI.Model;

public record CreateProductCommand(string Name, string Color, decimal Price) : IRequest<Product>;