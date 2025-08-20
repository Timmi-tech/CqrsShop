using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IRepositoryManager _repositoryManager;

        public CancelOrderHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.Order.GetOrderByIdAsync(request.OrderId, trackChanges: true) ?? throw new KeyNotFoundException("Order not found.");

            order.CancelOrder();

            // var productsIds = order.OrderItems.Select(oi => oi.ProductId).ToList();
            // var products = await _repositoryManager.Product.GetProductsByIdsAsync(productsIds, trackChanges: true);

            foreach (var item in order.OrderItems)
            {
                // var product = products.First(p => p.Id == item.ProductId);
                // product.AdjustStock(item.Quantity);

                item.Product.Inventory?.AdjustStock(item.Quantity);
            }
            await _repositoryManager.SaveAsync();

            return Unit.Value;
        }
    }
}