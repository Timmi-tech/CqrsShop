using Application.Interfaces.Contracts;
using Domain.Common;
using Domain.Entities.Models;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class PlaceOrderHandler(IRepositoryManager repositoryManager) : IRequestHandler<PlaceOrderCommand, Result<Guid>>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Result<Guid>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var Customer = await _repositoryManager.User.GetUserProfileAsync(request.CustomerId, trackChanges: false);
            if (Customer is null)
                return Result<Guid>.Failure(Error.NotFound("Customer", request.CustomerId));

            var productIds = request.Products.Select(p => p.ProductId).ToList();
            var products = await _repositoryManager.Product.GetProductsByIdsAsync(productIds, trackChanges: true);

            var foundProductIds = products.Select(p => p.Id).ToHashSet();
            if (!productIds.All(id => foundProductIds.Contains(id)))
                return Result<Guid>.Failure(Error.NotFound("Product", "One or more products not found."));


            var order = new Order(request.CustomerId);

            foreach (var item in request.Products)
            {
                var product = products.First(p => p.Id == item.ProductId);

                if (product.Inventory is null)
                    return Result<Guid>.Failure(Error.Validation("InventoryMissing", $"Product {product.Name} has no inventory."));

                if (!product.Inventory.HasSufficientStock(item.Quantity))
                    return Result<Guid>.Failure(Error.Validation("InsufficientStock", $"Insufficient stock for {product.Name}."));

                var adjustResult = product.Inventory.AdjustStock(-item.Quantity);    
                if (!adjustResult.IsSuccess)
                    return Result<Guid>.Failure(adjustResult.Error!);

                order.AddOrderItem(product.Id, item.Quantity, product.Price);
            }
            _repositoryManager.Order.CreateOrder(order);

            await _repositoryManager.SaveAsync();
            
            return Result<Guid>.Success(order.Id);
        }
    }
}
