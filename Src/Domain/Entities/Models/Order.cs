namespace Domain.Entities.Models
{
    public class Order(string customerId) : BaseEntity
    {
        public String CustomerId { get; private set; } = customerId;
        public User Customer { get; private set; } = null!;
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; private set; }

        // Navigation property for OrderItems
        public List<OrderItem> OrderItems { get; private set; } = new();

        public void AddOrderItem(Guid productId, int quantity, decimal price)
        {
            OrderItems.Add(new OrderItem(productId, quantity, price));
            TotalAmount = OrderItems.Sum(item => item.Quantity * item.Price);
        }
        public void MarkAsCompleted()
        {
            Status = OrderStatus.Completed;
        }
        public void CancelOrder()
        {
            if (Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel a completed order.");
            }
            Status = OrderStatus.Cancelled;
        }
    }
}