namespace Data.Users;

public class UserDto(int userId, string email, string username, string passwordHash, string role, DateTime createdAt)
{
    public int Id { get; } = userId;
    public string Email { get; } = email;
    public string Username { get; } = username;
    public string PasswordHash { get; } = passwordHash;
    public string Role { get; } = role;
    public DateTime CreatedAt { get; } = createdAt;
}