using Application.Common;
using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetProductsPagedQueryHandler : IRequestHandler<GetProductsPagedQuery, PagedResult<ProductDto>>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetProductsPagedQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<PagedResult<ProductDto>> Handle(GetProductsPagedQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _repositoryManager.Product.GetProductsPagedAsync(
                request.Pagination.PageNumber, 
                request.Pagination.PageSize, 
                trackChanges: false);

            var productDtos = products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Category,
                p.UserId,
                p.Inventory?.Quantity ?? 0
            ));

            return new PagedResult<ProductDto>
            {
                Items = productDtos,
                TotalCount = totalCount,
                PageNumber = request.Pagination.PageNumber,
                PageSize = request.Pagination.PageSize
            };
        }
    }
}