using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public record PlaceOrderCommand(string CustomerId, List<DTOs.OrderProductDto> Products) : IRequest<Guid>;
}