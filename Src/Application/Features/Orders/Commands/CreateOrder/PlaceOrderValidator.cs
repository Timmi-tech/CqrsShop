using FluentValidation;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class PlaceOrderValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.Products)
                .NotNull().WithMessage("Products list cannot be null.")
                .NotEmpty().WithMessage("At least one product must be included in the order.");

            RuleForEach(x => x.Products).ChildRules(product =>
            {
                product.RuleFor(p => p.ProductId)
                    .NotEmpty().WithMessage("Product ID is required.");

                product.RuleFor(p => p.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            });
        }
    }
}
