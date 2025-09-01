using Domain.Common;

namespace Domain.Entities.Models
{
    public class Inventory : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        // Navigation
        public Product? Product { get; private set; } 

        // Factory method
        public static Result<Inventory> Create(Guid productId, int initialQuantity)
        {
            if (initialQuantity < 0)
                 return Result<Inventory>.Failure(Error.Validation("Negative Stock", "Initial stock cannot be negative."));
                
                
            return Result<Inventory>.Success(new Inventory
            {
                ProductId = productId,
                Quantity = initialQuantity,
            });
        }
        public Result AdjustStock(int adjustment)
        {
            if (Quantity + adjustment < 0)
                return Result.Failure(Error.Validation("Negative Stock", "Stock cannot be negative."));

            Quantity += adjustment;
            UpdateTimestamp();
            return Result.Success();
        }
        public Result SetStock(int newQuantity)
        {
            if (newQuantity < 0)
                return Result.Failure(Error.Validation("Negative Stock", "Stock cannot be negative."));

            Quantity = newQuantity;
            UpdateTimestamp();
            return Result.Success();
        }
        public bool HasSufficientStock(int quantity) => Quantity >= quantity;
    }
}