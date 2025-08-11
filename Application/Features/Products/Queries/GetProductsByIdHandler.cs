using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetProductsByIdHandler : IRequestHandler<GetProductsByIdQuery, ProductDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        public GetProductsByIdHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ProductDto> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repositoryManager.Product.GetProductByIdAsync(request.Id, trackChanges: false) ?? throw new Exception($"Product with id {request.Id} not found");
            var productDto = new ProductDto
            (
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity,
                product.Category,
                product.UserId
            );

            return productDto;
        }
    }
} 