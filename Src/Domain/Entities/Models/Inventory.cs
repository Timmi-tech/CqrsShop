namespace Domain.Entities.Models
{
    public class Inventory : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        // Navigation
        public Product? Product { get; private set; } 

        // Factory method
        internal static Inventory Create(Guid productId, int initialQuantity)
        {
            if (initialQuantity < 0)
                throw new ArgumentException("Initial stock cannot be negative.");
                
            return new Inventory
            {
                ProductId = productId,
                Quantity = initialQuantity,
            };
        }
        public void AdjustStock(int adjustment)
        {
            if (Quantity + adjustment < 0)
                throw new InvalidOperationException("Stock cannot be negative.");

            Quantity += adjustment;
            UpdateTimestamp();
        }
        public void SetStock(int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidOperationException("Stock cannot be negative.");

            Quantity = newQuantity;
            UpdateTimestamp();
        }
        public bool HasSufficientStock(int quantity) => Quantity >= quantity;
    }
}