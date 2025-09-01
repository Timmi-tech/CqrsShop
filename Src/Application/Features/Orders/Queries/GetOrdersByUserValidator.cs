using FluentValidation;

namespace Application.Features.Orders.Queries
{
    public class GetOrdersByUserValidator : AbstractValidator<GetOrdersByUserQuery>
    {
        public GetOrdersByUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}