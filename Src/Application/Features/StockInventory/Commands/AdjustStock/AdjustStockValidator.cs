using FluentValidation;

namespace Application.Features.Commands.AdjustStock
{
    public class DecreaseStockValidator : AbstractValidator<DecreaseStockCommand>
    {
        public DecreaseStockValidator()
        {
            RuleFor(a => a.ProductId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(a => a.Quantity)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThanOrEqualTo(-1000).WithMessage("{PropertyName} must be greater than or equal to -1000.")
                .LessThanOrEqualTo(1000).WithMessage("{PropertyName} must be less than or equal to 1000.");
        }
    }
    public class IncreaseStockValidator : AbstractValidator<IncreaseStockCommand>
    {
        public IncreaseStockValidator()
        {
            RuleFor(a => a.ProductId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(a => a.Quantity)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThanOrEqualTo(-1000).WithMessage("{PropertyName} must be greater than or equal to -1000.")
                .LessThanOrEqualTo(1000).WithMessage("{PropertyName} must be less than or equal to 1000.");
        }
    }
}