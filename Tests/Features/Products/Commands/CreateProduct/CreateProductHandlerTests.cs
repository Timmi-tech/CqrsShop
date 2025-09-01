using Application.Features.Products.Commands.CreateProduct;
using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using FluentAssertions;
using Moq;

namespace Tests.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _repositoryManagerMock.Setup(rm => rm.Product).Returns(_productRepositoryMock.Object);
            _handler = new CreateProductHandler(_repositoryManagerMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ReturnsSuccessWithProductId()
        {
            // Arrange
            var command = new CreateProductCommand("Test Product", "Description", 29.99m, "Electronics", "user123", 10);
            _repositoryManagerMock.Setup(rm => rm.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
            _productRepositoryMock.Verify(p => p.CreateProduct(It.IsAny<Product>()), Times.Once);
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidPrice_ReturnsFailure()
        {
            // Arrange
            var command = new CreateProductCommand("Test Product", "Description", -10m, "Electronics", "user123", 10);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("Invalid Price");
            _productRepositoryMock.Verify(p => p.CreateProduct(It.IsAny<Product>()), Times.Never);
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WithNegativeInitialStock_ReturnsFailure()
        {
            // Arrange
            var command = new CreateProductCommand("Test Product", "Description", 29.99m, "Electronics", "user123", -5);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("NegativeStock");
            _productRepositoryMock.Verify(p => p.CreateProduct(It.IsAny<Product>()), Times.Never);
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WithEmptyName_ReturnsFailure()
        {
            // Arrange
            var command = new CreateProductCommand("", "Description", 29.99m, "Electronics", "user123", 10);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("Invalid Name");
            _productRepositoryMock.Verify(p => p.CreateProduct(It.IsAny<Product>()), Times.Never);
            _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Never);
        }
    }
}