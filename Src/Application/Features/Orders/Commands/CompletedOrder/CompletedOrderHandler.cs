using Application.Interfaces.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Orders.Commands.CompletedOrder
{
    public class CompletedOrderHandler(IRepositoryManager repositoryManager) : IRequestHandler<CompletedOrderCommand, Result>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Result> Handle(CompletedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.Order.GetOrderByIdAsync(request.OrderId, trackChanges: true);
            if (order is null)
                return Result.Failure(Error.NotFound("Order", request.OrderId.ToString()));

            order.MarkAsCompleted();

            await _repositoryManager.SaveAsync();
            return Result.Success();
        }
    }
}