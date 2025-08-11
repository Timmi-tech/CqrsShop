using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IRepositoryManager _repositoryManager;
        public GetAllProductsHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

       public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
{
    var products = await _repositoryManager.Product.GetAllProductsAsync(trackChanges: false);

    var productsDto = products
    .Select(product => new ProductDto(
        product!.Id,
        product.Name,
        product.Description,
        product.Price,
        product.StockQuantity,
        product.Category,
        product.UserId
    ));

    return productsDto;
}

    }
}