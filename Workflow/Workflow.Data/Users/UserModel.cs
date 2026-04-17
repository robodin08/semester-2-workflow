namespace Data.Users;

public class UserModel(int userId, string email, string passwordHash, string username, DateTime createdAt)
{
    public int Id { get; } = userId;
    public string Email { get; } = email;
    public string Username { get; } = username;
    public string PasswordHash { get; } = passwordHash;
    public DateTime CreatedAt { get; } = createdAt;
}