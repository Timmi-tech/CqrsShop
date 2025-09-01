using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Queries
{
    public record GetProductsPagedQuery(PaginationParameters Pagination) : IRequest<PagedResult<ProductDto>>;
}