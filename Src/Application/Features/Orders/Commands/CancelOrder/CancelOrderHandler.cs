using Application.Interfaces.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly IRepositoryManager _repositoryManager;

        public CancelOrderHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            
            var order = await _repositoryManager.Order.GetOrderByIdAsync(request.OrderId, trackChanges: true);

            if (order is null)
                return Result.Failure(Error.NotFound("Order", request.OrderId.ToString()));

            var cancelResult = order.CancelOrder();
            if (!cancelResult.IsSuccess)
                return cancelResult;

            foreach (var item in order.OrderItems)
            {
                if (item.Product.Inventory is not null)
                {
                    var adjustResult = item.Product.Inventory.AdjustStock(item.Quantity);
                    if (!adjustResult.IsSuccess)
                        return adjustResult;
                }
            }
            // Save changes
            await _repositoryManager.SaveAsync();

            return Result.Success();
        }
    }
}