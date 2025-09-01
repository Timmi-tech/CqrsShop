using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.StockInventory.Queries
{
    public class GetAllStockLevelsQueryHandler : IRequestHandler<GetAllStockLevelsQuery, List<StockLevelDto>>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetAllStockLevelsQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<List<StockLevelDto>> Handle(GetAllStockLevelsQuery request, CancellationToken cancellationToken)
        {
            var inventories = await _repositoryManager.Inventory.GetAllInventoriesAsync(trackChanges: false);
            
            return inventories.Select(inv => new StockLevelDto(
                inv.ProductId,
                inv.Product?.Name ?? "Unknown",
                inv.Quantity
            )).ToList();
        }
    }
}