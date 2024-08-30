using FluentValidation;

namespace ProductsWebAPI.Model;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Color).NotEmpty().WithMessage("Color is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required");
    }
}