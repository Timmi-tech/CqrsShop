using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Commands.AdjustStock
{
    public class  IncreaseStockCommandHandler(IRepositoryManager repositoryManager) : IRequestHandler<IncreaseStockCommand, Unit>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Unit> Handle(IncreaseStockCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _repositoryManager.Inventory
                .GetInventoryByProductIdAsync(request.ProductId, trackChanges: true)
                ?? throw new KeyNotFoundException($"Inventory not found for ProductId: {request.ProductId}");

            inventory.AdjustStock(request.Quantity);

            await _repositoryManager.SaveAsync();

            return Unit.Value;
        }
    }
    public class DecreaseStockCommandHandler(IRepositoryManager repositoryManager) : IRequestHandler<DecreaseStockCommand, Unit>
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task<Unit> Handle(DecreaseStockCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _repositoryManager.Inventory
                .GetInventoryByProductIdAsync(request.ProductId, trackChanges: true)
                ?? throw new KeyNotFoundException($"Inventory not found for ProductId: {request.ProductId}");

            inventory.AdjustStock(-request.Quantity);

            await _repositoryManager.SaveAsync();

            return Unit.Value;
        }
    }
    
}
