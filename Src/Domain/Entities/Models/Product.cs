namespace Domain.Entities.Models
{

    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        // public string ImageUrl { get; private set; }

        public User? Admin { get; private set;}
        public Inventory? Inventory { get; private set; }

        // Factory method for creation
        public static Product Create(string name, string description, decimal price, string category, string userId)
        {
            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                UserId = userId,
                // ImageUrl = imageUrl,
            };
        }
        public Inventory AddInventory(int initialStock)
        {
            if (initialStock < 0)
                throw new ArgumentException("Initial stock cannot be negative.");

            Inventory = Inventory.Create(Id, initialStock);
            return Inventory;
        }
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            Price = newPrice;
            UpdateTimestamp();
        }
    }
}
