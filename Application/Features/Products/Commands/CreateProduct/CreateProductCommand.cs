using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int StockQuantity,
        string Category,
        string UserId
    ) : IRequest<Guid>;
}