using Domain.Common;
using MediatR;

namespace Application.Features.Orders.Commands.CancelOrder
{
    public record CancelOrderCommand(Guid OrderId) : IRequest<Result>;
}