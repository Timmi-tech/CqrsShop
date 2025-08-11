namespace Domain.Entities.Models
{

    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        // public string ImageUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public User Admin { get; private set;} = null!;

        // Factory method for creation
        public static Product Create(string name, string description, decimal price, int stockQuantity, string category, string userId)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                StockQuantity = stockQuantity,
                Category = category,
                UserId = userId,
                // ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void AdjustStock(int quantity)
        {
            if (StockQuantity + quantity < 0)
                throw new InvalidOperationException("Stock cannot be negative.");

            StockQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            Price = newPrice;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool HasSufficientStock(int quantity) => StockQuantity >= quantity;
    }
}
