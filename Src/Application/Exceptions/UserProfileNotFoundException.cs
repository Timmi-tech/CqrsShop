using Application.Exceptions;

public sealed class UserProfileNotFoundException(string userId) : NotFoundException($"The User with id: {userId} doesn't exist in the database.")
{
}