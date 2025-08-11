using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : ReadRepository<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryWriteDbContext repositoryReadContextFactory) : base(repositoryReadContextFactory)
        {
        }

        // Get all orders
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges) => await FindAll(trackChanges)
        .Include(x => x.OrderItems)
            .ThenInclude(y => y.Product)
        .ToListAsync();

        // Get order by id
        public async Task<Order?> GetOrderByIdAsync(Guid id, bool trackChanges)
        {
            var query = FindByCondition(x => x.Id.Equals(id), trackChanges)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product);

            return await query.FirstOrDefaultAsync();
        }

        // Create Order
        public void CreateOrder(Order order) => Create(order);


    }
}