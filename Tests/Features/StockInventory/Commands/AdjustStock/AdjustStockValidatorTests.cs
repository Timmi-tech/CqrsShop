using Application.Features.Commands.AdjustStock;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Tests.Validators
{
    public class AdjustStockValidatorTests
    {
        [Fact]
        public void IncreaseStockValidator_WithValidCommand_PassesValidation()
        {
            // Arrange
            var validator = new IncreaseStockValidator();
            var command = new IncreaseStockCommand(Guid.NewGuid(), 10);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void IncreaseStockValidator_WithEmptyProductId_FailsValidation()
        {
            // Arrange
            var validator = new IncreaseStockValidator();
            var command = new IncreaseStockCommand(Guid.Empty, 10);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductId)
                  .WithErrorMessage("Product Id is required.");
        }

        [Fact]
        public void IncreaseStockValidator_WithZeroQuantity_FailsValidation()
        {
            // Arrange
            var validator = new IncreaseStockValidator();
            var command = new IncreaseStockCommand(Guid.NewGuid(), 0);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                  .WithErrorMessage("Quantity is required.");
        }

        [Theory]
        [InlineData(-1001)]
        [InlineData(1001)]
        public void IncreaseStockValidator_WithQuantityOutOfRange_FailsValidation(int quantity)
        {
            // Arrange
            var validator = new IncreaseStockValidator();
            var command = new IncreaseStockCommand(Guid.NewGuid(), quantity);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }

        [Fact]
        public void DecreaseStockValidator_WithValidCommand_PassesValidation()
        {
            // Arrange
            var validator = new DecreaseStockValidator();
            var command = new DecreaseStockCommand(Guid.NewGuid(), 5);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void DecreaseStockValidator_WithEmptyProductId_FailsValidation()
        {
            // Arrange
            var validator = new DecreaseStockValidator();
            var command = new DecreaseStockCommand(Guid.Empty, 5);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductId)
                  .WithErrorMessage("Product Id is required.");
        }

        [Fact]
        public void DecreaseStockValidator_WithZeroQuantity_FailsValidation()
        {
            // Arrange
            var validator = new DecreaseStockValidator();
            var command = new DecreaseStockCommand(Guid.NewGuid(), 0);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                  .WithErrorMessage("Quantity is required.");
        }

        [Theory]
        [InlineData(-1001)]
        [InlineData(1001)]
        public void DecreaseStockValidator_WithQuantityOutOfRange_FailsValidation(int quantity)
        {
            // Arrange
            var validator = new DecreaseStockValidator();
            var command = new DecreaseStockCommand(Guid.NewGuid(), quantity);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }
    }
}