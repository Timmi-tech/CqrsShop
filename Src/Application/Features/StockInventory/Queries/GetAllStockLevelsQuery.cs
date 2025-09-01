using MediatR;

namespace Application.Features.StockInventory.Queries
{
    public record GetAllStockLevelsQuery() : IRequest<List<StockLevelDto>>;

    public record StockLevelDto(Guid ProductId, string ProductName, int Quantity);
}