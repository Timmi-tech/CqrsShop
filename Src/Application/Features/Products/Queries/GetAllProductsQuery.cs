using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Queries
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
}