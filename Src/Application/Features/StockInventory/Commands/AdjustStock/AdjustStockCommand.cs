using MediatR;

namespace Application.Features.Commands.AdjustStock
{
    public record IncreaseStockCommand(Guid ProductId, int Quantity) : IRequest<Unit>;
    public record DecreaseStockCommand(Guid ProductId, int Quantity) : IRequest<Unit>;
}