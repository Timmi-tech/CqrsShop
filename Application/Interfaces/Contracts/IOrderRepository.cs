using Domain.Entities.Models;

namespace Application.Interfaces.Contracts
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges);
        void CreateOrder(Order order);
    }
}