using Application.Interfaces;
using Application.Interfaces.Contracts;
using Application.Interfaces.Services.Contracts;
using Domain.Entities.ConfigurationsModels;
using Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IUserProfileService> _userProfileService;
        public ServiceManager
        (
            ILoggerManager logger,
            UserManager<User> userManager,
            IOptions<JwtConfiguration> configuration,
            IRepositoryManager repositoryManager

        )
        {
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, userManager, configuration));
            _userProfileService = new Lazy<IUserProfileService>(() => new UserProfileService(repositoryManager, logger));
        }
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IUserProfileService UserProfileService => _userProfileService.Value;
    }
}