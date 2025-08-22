using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class PlaceOrderHandler(IRepositoryManager repositoryManager) : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var Customer = await _repositoryManager.User.GetUserProfileAsync(request.CustomerId, trackChanges: false) ?? throw new KeyNotFoundException("Customer not found.");


            var productIds = request.Products.Select(p => p.ProductId).ToList();
            var products = await _repositoryManager.Product.GetProductsByIdsAsync(productIds, trackChanges: true);

            var foundProductIds = products.Select(p => p.Id).ToHashSet();
            if (!productIds.All(id => foundProductIds.Contains(id)))
                throw new KeyNotFoundException("One or more products not found.");

            var order = new Order(request.CustomerId);

            foreach (var item in request.Products)
            {
                var product = products.First(p => p.Id == item.ProductId);

                if (product.Inventory == null || !product.Inventory.HasSufficientStock(item.Quantity))
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}.");
                // Reduce stock using AdjustStock with negative value
                product.Inventory.AdjustStock(-item.Quantity);    

                order.AddOrderItem(product.Id, item.Quantity, product.Price);
            }
            _repositoryManager.Order.CreateOrder(order);

            await _repositoryManager.SaveAsync();
            
            return order.Id;
        }
    }
}
