using Domain.Common;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string Category,
        bool TrackChanges = true
    ) : IRequest<Result>;
}