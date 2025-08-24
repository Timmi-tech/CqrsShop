    using Application.Interfaces.Contracts;
using Domain.Common;
using MediatR;

    namespace Application.Features.Commands.AdjustStock
    {
        public class  IncreaseStockCommandHandler(IRepositoryManager repositoryManager) : IRequestHandler<IncreaseStockCommand, Result>
        {
            private readonly IRepositoryManager _repositoryManager = repositoryManager;

            public async Task<Result> Handle(IncreaseStockCommand request, CancellationToken cancellationToken)
            {
            var inventory = await _repositoryManager.Inventory
                .GetInventoryByProductIdAsync(request.ProductId, trackChanges: true);

            if (inventory is null)
            {
                return Result.Failure(Error.NotFound("Inventory", request.ProductId.ToString()));
            }
                inventory.AdjustStock(request.Quantity);

                await _repositoryManager.SaveAsync();

                return Result.Success();
            }
        }
        public class DecreaseStockCommandHandler(IRepositoryManager repositoryManager) : IRequestHandler<DecreaseStockCommand, Result>
        {
            private readonly IRepositoryManager _repositoryManager = repositoryManager;

            public async Task<Result> Handle(DecreaseStockCommand request, CancellationToken cancellationToken)
            {
            var inventory = await _repositoryManager.Inventory
                .GetInventoryByProductIdAsync(request.ProductId, trackChanges: true);
            if (inventory is null)
            {
                return Result.Failure(Error.NotFound("Inventory", request.ProductId.ToString()));
            }

                inventory.AdjustStock(-request.Quantity);

                await _repositoryManager.SaveAsync();

                return Result.Success();
            }
        }
        
    }
