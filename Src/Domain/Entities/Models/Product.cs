using Domain.Common;

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
        public static Result<Product> Create(string name, string description, decimal price, string category, string userId)
        {

            if (price <= 0)
                return Result<Product>.Failure(Error.Validation("Invalid Price", "Price must be greater than zero."));

            if (string.IsNullOrWhiteSpace(name))
                return Result<Product>.Failure(Error.Validation("Invalid Name", "Product name cannot be empty."));

            Product product = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                UserId = userId,
                // ImageUrl = imageUrl,
            };
            return Result<Product>.Success(product);
        }
        public void Update(string name, string description, decimal price, string category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            }
        public Result<Inventory> AddInventory(int initialStock)
        {
            if (initialStock < 0)
                return Result<Inventory>.Failure(Error.Validation("NegativeStock", "Initial stock cannot be negative."));

            var result = Inventory.Create(Id, initialStock);
            if (!result.IsSuccess)
                return result;

            Inventory = result.Value;
            return Result<Inventory>.Success(Inventory!);
        }
        public Result UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                return Result.Failure(Error.Validation("InvalidPrice", "Price must be greater than zero."));

            Price = newPrice;
            UpdateTimestamp();
            return Result.Success();
        }
    }
    }
