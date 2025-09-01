using Application.Features.Commands.AdjustStock;
using Application.Features.StockInventory.Queries;
using Domain.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System.Security.Claims;

namespace Tests.Controllers
{
    public class InventoryControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InventoryController _controller;

        public InventoryControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new InventoryController(_mediatorMock.Object);
            SetupControllerContext();
        }

        private void SetupControllerContext()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, "Admin"),
                new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Fact]
        public async Task IncreaseStock_WithValidCommand_ReturnsNoContent()
        {
            // Arrange
            var command = new IncreaseStockCommand(Guid.NewGuid(), 10);
            _mediatorMock.Setup(m => m.Send(command, default))
                        .ReturnsAsync(Result.Success());

            // Act
            var result = await _controller.IncreaseStock(command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task IncreaseStock_WithFailedResult_ReturnsNotFound()
        {
            // Arrange
            var command = new IncreaseStockCommand(Guid.NewGuid(), 10);
            var error = Error.NotFound("Inventory", command.ProductId.ToString());
            _mediatorMock.Setup(m => m.Send(command, default))
                        .ReturnsAsync(Result.Failure(error));

            // Act
            var result = await _controller.IncreaseStock(command);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(error);
        }

        [Fact]
        public async Task DecreaseStock_WithValidCommand_ReturnsNoContent()
        {
            // Arrange
            var command = new DecreaseStockCommand(Guid.NewGuid(), 5);
            _mediatorMock.Setup(m => m.Send(command, default))
                        .ReturnsAsync(Result.Success());

            // Act
            var result = await _controller.DecreaseStock(command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task DecreaseStock_WithFailedResult_ReturnsNotFound()
        {
            // Arrange
            var command = new DecreaseStockCommand(Guid.NewGuid(), 5);
            var error = Error.NotFound("Inventory", command.ProductId.ToString());
            _mediatorMock.Setup(m => m.Send(command, default))
                        .ReturnsAsync(Result.Failure(error));

            // Act
            var result = await _controller.DecreaseStock(command);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(error);
        }

        [Fact]
        public async Task GetStockLevel_WithExistingProduct_ReturnsOkWithStockLevel()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var stockLevel = 25;
            _mediatorMock.Setup(m => m.Send(It.Is<GetStockLevelQuery>(q => q.ProductId == productId), default))
                        .ReturnsAsync(stockLevel);

            // Act
            var result = await _controller.GetStockLevel(productId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeEquivalentTo(new { ProductId = productId, Quantity = stockLevel });
        }

        [Fact]
        public async Task GetStockLevel_WithNonExistentProduct_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.Is<GetStockLevelQuery>(q => q.ProductId == productId), default))
                        .ReturnsAsync((int?)null);

            // Act
            var result = await _controller.GetStockLevel(productId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllStockLevels_ReturnsOkWithStockLevels()
        {
            // Arrange
            var stockLevels = new List<StockLevelDto>
            {
                new(Guid.NewGuid(), "Product 1", 10),
                new(Guid.NewGuid(), "Product 2", 20)
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllStockLevelsQuery>(), default))
                        .ReturnsAsync(stockLevels);

            // Act
            var result = await _controller.GetAllStockLevels();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(stockLevels);
        }

        [Fact]
        public async Task GetAllStockLevels_WithEmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var emptyList = new List<StockLevelDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllStockLevelsQuery>(), default))
                        .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAllStockLevels();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(emptyList);
        }
    }
}