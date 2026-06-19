using Data.Users;

namespace Workflow.Core.Users;

public class User(int id, Email email, Username username, string passwordHash, string role, DateTime createdAt)
{
    
    public int Id { get; } = id;
    public Email Email { get; } = email;
    public Username Username { get; } = username;
    public string PasswordHash { get; } = passwordHash;
    public string Role { get; } = role;
    public DateTime CreatedAt { get; } = createdAt;

    public static User FromUserDto(UserDto dto)
    {
        return new User(dto.Id, new Email(dto.Email), new Username(dto.Username), dto.PasswordHash, dto.Role, dto.CreatedAt);
    }
}