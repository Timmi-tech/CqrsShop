using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetAllProductsHandler(IRepositoryManager repositoryManager) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
{
    var products = await _repositoryManager.Product.GetAllProductsAsync(trackChanges: false);
    

    var productsDto = products
    .Select(product => new ProductDto(
        product!.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Category,
        product.UserId,
        product.Inventory?.Quantity ?? 0 // <-- return stock
    ));

    return productsDto;
}

    }
}