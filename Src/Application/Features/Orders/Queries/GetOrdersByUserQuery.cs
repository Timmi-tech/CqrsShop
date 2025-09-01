using Application.DTOs;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public record GetOrdersByUserQuery(string UserId) : IRequest<List<OrderDto>>;
}