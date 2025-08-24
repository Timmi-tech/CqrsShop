using Application.Interfaces.Contracts;
using Domain.Common;
using Domain.Entities.Models;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler(IRepositoryManager repositoryManager) : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Create Product
            Result<Product> productResult  = Product.Create(
                request.Name,
                request.Description,
                request.Price,
                request.Category,
                request.UserId
            );

            if (!productResult.IsSuccess)
                return Result<Guid>.Failure(productResult.Error!);

            var product = productResult.Value!;
                
             // 2. Add Inventory inside Product aggregate
            var inventoryResult = product.AddInventory(request.InitialStock);
            if (!inventoryResult.IsSuccess)
                return Result<Guid>.Failure(inventoryResult.Error!);

             _repositoryManager.Product.CreateProduct(product);

            // 3. Save changes to the database
            await _repositoryManager.SaveAsync();

            return Result<Guid>.Success(product.Id);
        }
    }
    
}