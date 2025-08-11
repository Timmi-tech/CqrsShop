using FluentValidation;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 1000 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal to 0.");

            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}