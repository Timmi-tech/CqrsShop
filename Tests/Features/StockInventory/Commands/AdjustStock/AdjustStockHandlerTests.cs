using Application.Features.Commands.AdjustStock;
using Application.Interfaces.Contracts;
using Domain.Common;
using Domain.Entities.Models;
using FluentAssertions;
using Moq;

namespace Tests.Handlers
{
    public class AdjustStockHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;

        public AdjustStockHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _repositoryManagerMock.Setup(rm => rm.Inventory).Returns(_inventoryRepositoryMock.Object);
        }

        [Fact]
        public async Task IncreaseStockHandler_WithValidInventory_ReturnsSuccess()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new IncreaseStockCommand(productId, 10);
            var inventory = CreateInventory(productId, 20);
            
            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, true))
                                   .ReturnsAsync(inventory);
            _repositoryManagerMock.Setup(rm => rm.SaveAsync()).Returns(Task.CompletedTask);

            var handler = new IncreaseStockCommandHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task IncreaseStockHandler_WithNonExistentInventory_ReturnsFailure()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new IncreaseStockCommand(productId, 10);
            
            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, true))
                                   .ReturnsAsync((Inventory?)null);

            var handler = new IncreaseStockCommandHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("NotFound");
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DecreaseStockHandler_WithSufficientStock_ReturnsSuccess()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DecreaseStockCommand(productId, 5);
            var inventory = CreateInventory(productId, 20);
            
            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, true))
                                   .ReturnsAsync(inventory);
            _repositoryManagerMock.Setup(rm => rm.SaveAsync()).Returns(Task.CompletedTask);

            var handler = new DecreaseStockCommandHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DecreaseStockHandler_WithInsufficientStock_ReturnsFailure()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DecreaseStockCommand(productId, 25);
            var inventory = CreateInventory(productId, 20);
            
            // The handler calls AdjustStock with negative value, so we need to simulate that failure
            // Since we can't mock the inventory's AdjustStock method, let's create inventory with less stock
            inventory = CreateInventory(productId, 5); // This will cause insufficient stock
            
            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, true))
                                   .ReturnsAsync(inventory);

            var handler = new DecreaseStockCommandHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("Negative Stock");
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DecreaseStockHandler_WithNonExistentInventory_ReturnsFailure()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DecreaseStockCommand(productId, 5);
            
            _inventoryRepositoryMock.Setup(r => r.GetInventoryByProductIdAsync(productId, true))
                                   .ReturnsAsync((Inventory?)null);

            var handler = new DecreaseStockCommandHandler(_repositoryManagerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("NotFound");
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Never);
        }

        private static Inventory CreateInventory(Guid productId, int quantity)
        {
            var inventoryResult = Inventory.Create(productId, quantity);
            return inventoryResult.Value!;
        }
    }
}