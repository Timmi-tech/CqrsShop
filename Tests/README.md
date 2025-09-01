# CqrsShop Tests

This project contains comprehensive unit and integration tests for the CqrsShop application, focusing on the inventory management functionality.

## Test Structure

```
Tests/
├── Controllers/           # Controller unit tests
├── Handlers/             # MediatR handler unit tests  
├── Domain/               # Domain model unit tests
├── Validators/           # FluentValidation tests
├── Integration/          # Integration tests
└── GlobalUsings.cs       # Global test imports
```

## Test Categories

### Controller Tests (`InventoryControllerTests`)
- Tests all InventoryController endpoints
- Mocks MediatR to isolate controller logic
- Validates HTTP responses and status codes
- Tests authentication/authorization scenarios

### Handler Tests
- **`AdjustStockHandlerTests`**: Tests stock increase/decrease command handlers
- **`StockQueryHandlerTests`**: Tests stock level query handlers
- Mocks repository dependencies
- Validates business logic and error handling

### Domain Tests (`InventoryTests`)
- Tests Inventory entity business rules
- Validates stock adjustment logic
- Tests factory methods and validation
- Ensures domain invariants are maintained

### Validator Tests (`AdjustStockValidatorTests`)
- Tests FluentValidation rules for commands
- Validates input constraints and error messages
- Uses FluentValidation test helpers

### Integration Tests (`InventoryControllerIntegrationTests`)
- End-to-end HTTP endpoint testing
- Tests authentication/authorization middleware
- Uses WebApplicationFactory for realistic testing

## Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test category
dotnet test --filter "Category=Unit"
```

## Test Dependencies

- **xUnit**: Test framework
- **Moq**: Mocking framework
- **FluentAssertions**: Fluent assertion library
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing
- **FluentValidation.TestHelper**: Validation testing utilities

## Best Practices Followed

1. **AAA Pattern**: Arrange, Act, Assert structure
2. **Descriptive Names**: Test names clearly describe scenarios
3. **Single Responsibility**: Each test validates one specific behavior
4. **Mocking**: External dependencies are mocked appropriately
5. **Test Data**: Uses realistic test data and edge cases
6. **Isolation**: Tests are independent and can run in any order