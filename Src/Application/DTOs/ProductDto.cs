namespace Application.DTOs
{
    public record ProductDto
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string Category,
        string UserId,
        int StockQuantity
    );
}