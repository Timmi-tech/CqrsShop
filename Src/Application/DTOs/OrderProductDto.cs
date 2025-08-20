namespace Application.DTOs
{
    public record OrderProductDto(
        Guid ProductId,
        int Quantity
    );

    public record OrderDto(
    Guid Id,
    string Status,
    decimal TotalPrice,
    List<OrderProductReadDto> Products,
    DateTime OrderDate
);

    public record OrderProductReadDto(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
        );
}