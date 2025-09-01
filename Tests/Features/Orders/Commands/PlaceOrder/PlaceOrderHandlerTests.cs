using Application.DTOs;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using FluentAssertions;
using Moq;

namespace Tests.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IUserProfileRepository> _userRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly PlaceOrderHandler _handler;

        public PlaceOrderHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _userRepositoryMock = new Mock<IUserProfileRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            
            _repositoryManagerMock.Setup(rm => rm.User).Returns(_userRepositoryMock.Object);
            _repositoryManagerMock.Setup(rm => rm.Product).Returns(_productRepositoryMock.Object);
            _repositoryManagerMock.Setup(rm => rm.Order).Returns(_orderRepositoryMock.Object);
            
            _handler = new PlaceOrderHandler(_repositoryManagerMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidOrder_ReturnsSuccessWithOrderId()
        {
            // Arrange
            var customerId = "customer123";
            var productId = Guid.NewGuid();
            var customer = new User { Id = customerId };
            var inventory = CreateInventory(productId, 10);
            var product = CreateProductWithInventory(productId, "Test Product", 29.99m, inventory);
            
            var orderProducts = new List<OrderProductDto>
            {
                new(productId, 2)
            };
            var command = new PlaceOrderCommand(customerId, orderProducts);

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(customerId, false))
                              .ReturnsAsync(customer);
            _productRepositoryMock.Setup(x => x.GetProductsByIdsAsync(It.IsAny<List<Guid>>(), true))
                                 .ReturnsAsync(new List<Product> { product });
            _repositoryManagerMock.Setup(rm => rm.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
            _orderRepositoryMock.Verify(o => o.CreateOrder(It.IsAny<Order>()), Times.Once);
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentCustomer_ReturnsFailure()
        {
            // Arrange
            var customerId = "nonexistent";
            var orderProducts = new List<OrderProductDto>
            {
                new(Guid.NewGuid(), 2)
            };
            var command = new PlaceOrderCommand(customerId, orderProducts);

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(customerId, false))
                              .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("NotFound");
            _orderRepositoryMock.Verify(o => o.CreateOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WithInsufficientStock_ReturnsFailure()
        {
            // Arrange
            var customerId = "customer123";
            var productId = Guid.NewGuid();
            var customer = new User { Id = customerId };
            var inventory = CreateInventory(productId, 1); // Only 1 in stock
            var product = CreateProductWithInventory(productId, "Test Product", 29.99m, inventory);
            
            var orderProducts = new List<OrderProductDto>
            {
                new(productId, 5) // Requesting 5, but only 1 available
            };
            var command = new PlaceOrderCommand(customerId, orderProducts);

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(customerId, false))
                              .ReturnsAsync(customer);
            _productRepositoryMock.Setup(x => x.GetProductsByIdsAsync(It.IsAny<List<Guid>>(), true))
                                 .ReturnsAsync(new List<Product> { product });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("InsufficientStock");
            _orderRepositoryMock.Verify(o => o.CreateOrder(It.IsAny<Order>()), Times.Never);
        }

        private static Inventory CreateInventory(Guid productId, int quantity)
        {
            var inventoryResult = Inventory.Create(productId, quantity);
            return inventoryResult.Value!;
        }

        private static Product CreateProductWithInventory(Guid productId, string name, decimal price, Inventory inventory)
        {
            var productResult = Product.Create(name, "Test Description", price, "Test Category", "user123");
            var product = productResult.Value!;
            
            // Set the ID and inventory using reflection for testing
            var idProperty = typeof(Product).GetProperty("Id");
            idProperty?.SetValue(product, productId);
            
            var inventoryProperty = typeof(Product).GetProperty("Inventory");
            inventoryProperty?.SetValue(product, inventory);
            
            return product;
        }
    }
}