using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetProductsByIdHandler : IRequestHandler<GetProductsByIdQuery, IEnumerable<ProductDto>>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetProductsByIdHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
        {
            var products = await _repositoryManager.Product
                .GetProductsByIdsAsync(request.ProductIds, trackChanges: false);

            if (!products.Any())
                throw new Exception($"No products found for the given IDs.");

            return products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.StockQuantity,
                p.Category,
                p.UserId
            ));
        }
    }
}
