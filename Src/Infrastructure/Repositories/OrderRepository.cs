using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository(RepositoryWriteDbContext repositoryReadContextFactory) : ReadRepository<Order>(repositoryReadContextFactory), IOrderRepository
    {

        // Get all orders
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges) => await FindAll(trackChanges)
        .Include(x => x.OrderItems)
            .ThenInclude(y => y.Product)
        .ToListAsync();

        // Get order by id
        public async Task<Order?> GetOrderByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(id), trackChanges)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p.Inventory)
            .FirstOrDefaultAsync();
        }
        // Get orders by user id
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId, bool trackChanges) => 
            await FindByCondition(x => x.CustomerId.Equals(userId), trackChanges)
                .Include(x => x.OrderItems)
                    .ThenInclude(y => y.Product)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

        // Create Order
        public void CreateOrder(Order order) => Create(order);
    }
}