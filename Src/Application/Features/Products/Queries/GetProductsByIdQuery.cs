using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Queries
{
    public record GetProductsByIdQuery(IEnumerable<Guid> ProductIds) : IRequest<IEnumerable<ProductDto>>;
}