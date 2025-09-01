using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.ConfigurationsModels;
using Domain.Entities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<ILoggerManager> _loggerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IOptions<JwtConfiguration>> _jwtConfigMock;
        private readonly AuthenticationService _authService;
        private readonly JwtConfiguration _jwtConfig;

        public AuthenticationServiceTests()
        {
            _loggerMock = new Mock<ILoggerManager>();
            _userManagerMock = CreateMockUserManager();
            _jwtConfigMock = new Mock<IOptions<JwtConfiguration>>();
            
            _jwtConfig = new JwtConfiguration
            {
                Secret = "ThisIsAVeryLongSecretKeyForJWTTokenGeneration123456789",
                ValidIssuer = "TestIssuer",
                ValidAudience = "TestAudience",
                Expires = "60"
            };
            
            _jwtConfigMock.Setup(x => x.Value).Returns(_jwtConfig);
            _authService = new AuthenticationService(_loggerMock.Object, _userManagerMock.Object, _jwtConfigMock.Object);
        }

        [Fact]
        public async Task RegisterUser_WithValidData_ReturnsSuccessResult()
        {
            // Arrange
            var userDto = new UserForRegistrationDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Username = "johndoe",
                Email = "john@test.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterUser(userDto);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), userDto.Password), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var user = new User { Email = "test@test.com", UserName = "testuser" };
            var authDto = new UserForAuthenticationDto { Email = "test@test.com", Password = "Password123!" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(authDto.Email))
                           .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, authDto.Password))
                           .ReturnsAsync(true);

            // Act
            var result = await _authService.ValidateUser(authDto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(authDto.Email);
        }

        [Fact]
        public async Task ValidateUser_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var authDto = new UserForAuthenticationDto { Email = "invalid@test.com", Password = "Password123!" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(authDto.Email))
                           .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.ValidateUser(authDto);

            // Assert
            result.Should().BeNull();
            _loggerMock.Verify(x => x.LogWarn(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateToken_WithValidUser_ReturnsTokenDto()
        {
            // Arrange
            var user = new User 
            { 
                Id = Guid.NewGuid().ToString(),
                Email = "test@test.com", 
                UserName = "testuser",
                Role = UserRole.Customer
            };

            _userManagerMock.Setup(x => x.UpdateAsync(user))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.CreateToken(user, true);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        private static Mock<UserManager<User>> CreateMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }
    }
}