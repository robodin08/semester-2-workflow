namespace Data.Users;

public class RegisterDto(string email, string username, string passwordHash)
{
    public string Email { get; } = email;
    public string Username { get; } = username;
    public string PasswordHash { get; } = passwordHash;
}