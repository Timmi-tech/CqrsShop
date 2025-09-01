using Application.Features.StockInventory.Queries;
using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using FluentAssertions;
using Moq;

namespace Tests.Handlers
{
    public class StockQueryHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;

        public StockQueryHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _repositoryManagerMock.Setup(rm => rm.Inventory).Returns(_inventoryRepositoryMock.Object);
        }

        [Fact]
        public async Task GetStockLevelHandler_WithExistingInventory_ReturnsQuantity()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedQuantity = 25;
            var query = new GetStockLevelQuery(productId);
            var inventory = CreateInventory(productId, expectedQuantity);

            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, false))
                                   .ReturnsAsync(inventory);

            var handler = new GetStockLevelQueryHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().Be(expectedQuantity);
        }

        [Fact]
        public async Task GetStockLevelHandler_WithNonExistentInventory_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var query = new GetStockLevelQuery(productId);

            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, false))
                                   .ReturnsAsync((Inventory?)null);

            var handler = new GetStockLevelQueryHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllStockLevelsHandler_WithInventories_ReturnsStockLevelDtos()
        {
            // Arrange
            var query = new GetAllStockLevelsQuery();
            var inventories = new List<Inventory>
            {
                CreateInventoryWithProduct(Guid.NewGuid(), "Product 1", 10),
                CreateInventoryWithProduct(Guid.NewGuid(), "Product 2", 20)
            };

            _inventoryRepositoryMock.Setup(r => r.GetAllInventoriesAsync(false))
                                   .ReturnsAsync(inventories);

            var handler = new GetAllStockLevelsQueryHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result[0].ProductName.Should().Be("Product 1");
            result[0].Quantity.Should().Be(10);
            result[1].ProductName.Should().Be("Product 2");
            result[1].Quantity.Should().Be(20);
        }

        [Fact]
        public async Task GetAllStockLevelsHandler_WithEmptyInventories_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetAllStockLevelsQuery();
            var emptyInventories = new List<Inventory>();

            _inventoryRepositoryMock.Setup(r => r.GetAllInventoriesAsync(false))
                                   .ReturnsAsync(emptyInventories);

            var handler = new GetAllStockLevelsQueryHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllStockLevelsHandler_WithNullProductName_ReturnsUnknown()
        {
            // Arrange
            var query = new GetAllStockLevelsQuery();
            var inventory = CreateInventory(Guid.NewGuid(), 15);
            var inventories = new List<Inventory> { inventory };

            _inventoryRepositoryMock.Setup(r => r.GetAllInventoriesAsync(false))
                                   .ReturnsAsync(inventories);

            var handler = new GetAllStockLevelsQueryHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].ProductName.Should().Be("Unknown");
            result[0].Quantity.Should().Be(15);
        }

        private static Inventory CreateInventory(Guid productId, int quantity)
        {
            var inventoryResult = Inventory.Create(productId, quantity);
            return inventoryResult.Value!;
        }

        private static Inventory CreateInventoryWithProduct(Guid productId, string productName, int quantity)
        {
            var inventory = CreateInventory(productId, quantity);
            
            // Use reflection to set the Product property for testing
            var productProperty = typeof(Inventory).GetProperty("Product");
            var product = CreateProduct(productId, productName);
            productProperty?.SetValue(inventory, product);
            
            return inventory;
        }

        private static Product CreateProduct(Guid productId, string name)
        {
            // Create a minimal product for testing
            var productResult = Product.Create(name, "Test Description", 10.0m, "Test Category", "test-user-id");
            var product = productResult.Value!;
            
            // Set the ID using reflection
            var idProperty = typeof(Product).GetProperty("Id");
            idProperty?.SetValue(product, productId);
            
            return product;
        }
    }
}