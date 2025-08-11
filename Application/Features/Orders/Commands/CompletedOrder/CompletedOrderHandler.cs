using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Orders.Commands.CompletedOrder
{
    public class CompletedOrderHandler : IRequestHandler<CompletedOrderCommand, Unit>
    {
        private readonly IRepositoryManager _repositoryManager;

        public CompletedOrderHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Unit> Handle(CompletedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.Order.GetOrderByIdAsync(request.OrderId, trackChanges: true) ?? throw new KeyNotFoundException("Order not found.");

            order.MarkAsCompleted();

            await _repositoryManager.SaveAsync();
            return Unit.Value;
        }
    }
}