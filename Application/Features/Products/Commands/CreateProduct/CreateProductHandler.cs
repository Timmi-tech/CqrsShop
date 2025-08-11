using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IRepositoryManager _repositoryManager;

        public CreateProductHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity,
                request.Category,
                request.UserId
            );

            _repositoryManager.Product.CreateProduct(product);
            await _repositoryManager.SaveAsync();

            return product.Id;
        }
    }
}