using FluentValidation;

namespace Application.Features.Products.Queries
{
    public class GetProductsByIdValidator : AbstractValidator<GetProductsByIdQuery>
    {
        public GetProductsByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}