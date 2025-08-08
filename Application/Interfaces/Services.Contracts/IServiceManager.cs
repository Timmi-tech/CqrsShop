namespace Application.Interfaces.Services.Contracts
{
    public interface IServiceManager
    {
        IAuthenticationService AuthenticationService { get; }
        IUserProfileService UserProfileService { get; }
    }   
}