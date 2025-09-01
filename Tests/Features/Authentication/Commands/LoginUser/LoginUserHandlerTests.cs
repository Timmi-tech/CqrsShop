using Application.DTOs;
using Application.Features.Authentication.Commands.LoginUser;
using Application.Features.Authentication.Handlers;
using Application.Interfaces.Services.Contracts;
using Domain.Entities.Models;
using FluentAssertions;
using Moq;

namespace Tests.Features.Authentication.Commands.LoginUser
{
    public class LoginUserHandlerTests
    {
        private readonly Mock<IServiceManager> _serviceManagerMock;
        private readonly Mock<IAuthenticationService> _authServiceMock;
        private readonly LoginUserHandler _handler;

        public LoginUserHandlerTests()
        {
            _serviceManagerMock = new Mock<IServiceManager>();
            _authServiceMock = new Mock<IAuthenticationService>();
            _serviceManagerMock.Setup(x => x.AuthenticationService).Returns(_authServiceMock.Object);
            _handler = new LoginUserHandler(_serviceManagerMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCredentials_ReturnsSuccessWithToken()
        {
            // Arrange
            var user = new User { Email = "test@test.com", UserName = "testuser" };
            var token = new TokenDto("access-token", "refresh-token");
            var authDto = new UserForAuthenticationDto { Email = "test@test.com", Password = "Password123!" };
            var command = new LoginUserCommand(authDto);

            _authServiceMock.Setup(x => x.ValidateUser(authDto))
                           .ReturnsAsync(user);
            _authServiceMock.Setup(x => x.CreateToken(user, true))
                           .ReturnsAsync(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(token);
            _authServiceMock.Verify(x => x.ValidateUser(authDto), Times.Once);
            _authServiceMock.Verify(x => x.CreateToken(user, true), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var authDto = new UserForAuthenticationDto { Email = "invalid@test.com", Password = "WrongPassword" };
            var command = new LoginUserCommand(authDto);

            _authServiceMock.Setup(x => x.ValidateUser(authDto))
                           .ReturnsAsync((User)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be("InvalidCredentials");
            result.Error.Message.Should().Be("Email or password is incorrect");
            _authServiceMock.Verify(x => x.ValidateUser(authDto), Times.Once);
            _authServiceMock.Verify(x => x.CreateToken(It.IsAny<User>(), It.IsAny<bool>()), Times.Never);
        }
    }
}