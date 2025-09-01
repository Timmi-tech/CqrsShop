using Domain.Entities.Models;
using FluentAssertions;

namespace Tests.Domain
{
    public class InventoryTests
    {
        [Fact]
        public void Create_WithValidParameters_ReturnsSuccessResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var initialQuantity = 10;

            // Act
            var result = Inventory.Create(productId, initialQuantity);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value!.ProductId.Should().Be(productId);
            result.Value.Quantity.Should().Be(initialQuantity);
        }

        [Fact]
        public void Create_WithNegativeQuantity_ReturnsFailureResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var negativeQuantity = -5;

            // Act
            var result = Inventory.Create(productId, negativeQuantity);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("Negative Stock");
            result.Error.Message.Should().Be("Initial stock cannot be negative.");
        }

        [Fact]
        public void AdjustStock_WithPositiveAdjustment_IncreasesQuantity()
        {
            // Arrange
            var inventory = CreateInventory(20);
            var adjustment = 10;

            // Act
            var result = inventory.AdjustStock(adjustment);

            // Assert
            result.IsSuccess.Should().BeTrue();
            inventory.Quantity.Should().Be(30);
        }

        [Fact]
        public void AdjustStock_WithNegativeAdjustment_DecreasesQuantity()
        {
            // Arrange
            var inventory = CreateInventory(20);
            var adjustment = -5;

            // Act
            var result = inventory.AdjustStock(adjustment);

            // Assert
            result.IsSuccess.Should().BeTrue();
            inventory.Quantity.Should().Be(15);
        }

        [Fact]
        public void AdjustStock_WithAdjustmentCausingNegativeStock_ReturnsFailure()
        {
            // Arrange
            var inventory = CreateInventory(10);
            var adjustment = -15;

            // Act
            var result = inventory.AdjustStock(adjustment);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("Negative Stock");
            result.Error.Message.Should().Be("Stock cannot be negative.");
            inventory.Quantity.Should().Be(10); // Should remain unchanged
        }

        [Fact]
        public void SetStock_WithValidQuantity_UpdatesQuantity()
        {
            // Arrange
            var inventory = CreateInventory(20);
            var newQuantity = 50;

            // Act
            var result = inventory.SetStock(newQuantity);

            // Assert
            result.IsSuccess.Should().BeTrue();
            inventory.Quantity.Should().Be(newQuantity);
        }

        [Fact]
        public void SetStock_WithNegativeQuantity_ReturnsFailure()
        {
            // Arrange
            var inventory = CreateInventory(20);
            var negativeQuantity = -10;

            // Act
            var result = inventory.SetStock(negativeQuantity);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("Negative Stock");
            result.Error.Message.Should().Be("Stock cannot be negative.");
            inventory.Quantity.Should().Be(20); // Should remain unchanged
        }

        [Theory]
        [InlineData(20, 10, true)]
        [InlineData(20, 20, true)]
        [InlineData(20, 25, false)]
        [InlineData(0, 1, false)]
        public void HasSufficientStock_WithVariousQuantities_ReturnsExpectedResult(
            int currentStock, int requestedQuantity, bool expectedResult)
        {
            // Arrange
            var inventory = CreateInventory(currentStock);

            // Act
            var result = inventory.HasSufficientStock(requestedQuantity);

            // Assert
            result.Should().Be(expectedResult);
        }

        private static Inventory CreateInventory(int quantity)
        {
            var productId = Guid.NewGuid();
            var result = Inventory.Create(productId, quantity);
            return result.Value!;
        }
    }
}