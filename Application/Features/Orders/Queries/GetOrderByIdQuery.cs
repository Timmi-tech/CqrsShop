using Application.DTOs;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>;
}