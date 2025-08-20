using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        public GetOrderByIdQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.Order.GetOrderByIdAsync(request.OrderId, trackChanges: false)
                ?? throw new KeyNotFoundException("Order not found.");

            var dto = new OrderDto(
                order.Id,
                order.Status.ToString(),
                order.TotalAmount,
                order.OrderItems.Select(oi => new OrderProductReadDto(
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Quantity,
                    oi.Price
                )).ToList(),
                order.OrderDate
            );

            return dto;
        }

    }
}