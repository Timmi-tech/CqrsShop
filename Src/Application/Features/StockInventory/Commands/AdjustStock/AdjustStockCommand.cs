using Domain.Common;
using MediatR;

namespace Application.Features.Commands.AdjustStock
{
    public record IncreaseStockCommand(Guid ProductId, int Quantity) : IRequest<Result>;
    public record DecreaseStockCommand(Guid ProductId, int Quantity) : IRequest<Result>;
}