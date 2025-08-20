namespace Domain.Entities.Models
{
    public class OrderItem(Guid productId, int quantity, decimal price) : BaseEntity
    {
        public Guid ProductId { get; private set; } = productId;

        public Product Product { get; private set; } = null!;
        public int Quantity { get; private set; } = quantity;
        public decimal Price { get; private set; } = price;
    }
}