using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        string Category,
        string UserId,
        int InitialStock
    ) : IRequest<Guid>;
}