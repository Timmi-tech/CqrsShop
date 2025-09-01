using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.StockInventory.Queries
{
    public class GetStockLevelQueryHandler : IRequestHandler<GetStockLevelQuery, int?>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetStockLevelQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<int?> Handle(GetStockLevelQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _repositoryManager.Inventory.GetInventoryByProductIdAsync(request.ProductId, trackChanges: false);
            return inventory?.Quantity;
        }
    }
}