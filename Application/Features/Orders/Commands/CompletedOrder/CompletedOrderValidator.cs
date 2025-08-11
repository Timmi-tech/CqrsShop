using FluentValidation;

namespace Application.Features.Orders.Commands.CompletedOrder
{
    public class CompletedOrderValidator : AbstractValidator<CompletedOrderCommand>
    {
        public CompletedOrderValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}