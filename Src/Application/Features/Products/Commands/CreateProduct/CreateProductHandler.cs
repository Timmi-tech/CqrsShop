using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler(IRepositoryManager repositoryManager) : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Create Product
            Product product = Product.Create(
                request.Name,
                request.Description,
                request.Price,
                request.Category,
                request.UserId
            );
            
             // 2. Add Inventory inside Product aggregate
            product.AddInventory(request.InitialStock);

             _repositoryManager.Product.CreateProduct(product);

            // 3. Save changes to the database
            await _repositoryManager.SaveAsync();

            return product.Id;
        }
    }
    
}