using Application.Interfaces;
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

        public ServiceManager
        (
            ILoggerManager logger,
            UserManager<User> userManager,
            IOptions<JwtConfiguration> configuration
        )
        {
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, userManager,configuration));
        }
           public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}