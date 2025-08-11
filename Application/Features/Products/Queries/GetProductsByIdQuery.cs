using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Queries
{
    public record GetProductsByIdQuery(Guid Id) : IRequest<ProductDto>;
}