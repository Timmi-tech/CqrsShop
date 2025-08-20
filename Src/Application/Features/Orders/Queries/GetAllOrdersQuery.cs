using Application.DTOs;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public record GetAllOrdersQuery() : IRequest<List<OrderDto>>;
}