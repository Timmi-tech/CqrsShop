namespace Application.DTOs
{
    public record UserPofileDto
    {
        public string Id { get; init; } = string.Empty;
        public string Firstname { get; init; } = string.Empty;
        public string Lastname { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
    public record UserUpdateProfileDto
    {
        public string Firstname { get; init; } = string.Empty;
        public string Lastname { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;

    } 
}