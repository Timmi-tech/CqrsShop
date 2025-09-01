using MediatR;

namespace Application.Features.StockInventory.Queries
{
    public record GetStockLevelQuery(Guid ProductId) : IRequest<int?>;
}