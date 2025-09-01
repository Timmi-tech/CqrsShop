using Application.DTOs;
using Application.Interfaces.Contracts;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public class GetOrdersByUserQueryHandler : IRequestHandler<GetOrdersByUserQuery, List<OrderDto>>
    {
        private readonly IRepositoryManager _repositoryManager;

        public GetOrdersByUserQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repositoryManager.Order.GetOrdersByUserIdAsync(request.UserId, trackChanges: false);

            var dto = orders.Select(order => new OrderDto(
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
            )).ToList();
            
            return dto;
        }
    }
}