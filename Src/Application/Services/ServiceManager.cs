using Application.Interfaces;
using Application.Interfaces.Contracts;
using Application.Interfaces.Services.Contracts;
using Domain.Entities.ConfigurationsModels;
using Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public sealed class ServiceManager(
        ILoggerManager logger,
        UserManager<User> userManager,
        IOptions<JwtConfiguration> configuration,
        IRepositoryManager repositoryManager

        ) : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authenticationService = new(() => new AuthenticationService(logger, userManager, configuration));
        private readonly Lazy<IUserProfileService> _userProfileService = new(() => new UserProfileService(repositoryManager, logger));

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IUserProfileService UserProfileService => _userProfileService.Value;
    }
}