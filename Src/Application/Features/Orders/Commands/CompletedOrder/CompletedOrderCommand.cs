using Domain.Common;
using MediatR;

namespace Application.Features.Orders.Commands.CompletedOrder
{
    public record CompletedOrderCommand(Guid OrderId) : IRequest<Result>;
}