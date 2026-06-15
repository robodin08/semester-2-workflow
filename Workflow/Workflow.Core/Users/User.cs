using Data.Users;

namespace Workflow.Core.Users;

public class User(int id, Email email, Username username, string passwordHash, DateTime createdAt)
{
    
    public int Id { get; } = id;
    public Email Email { get; } = email;
    public Username Username { get; } = username;
    public string PasswordHash { get; } = passwordHash;
    public DateTime CreatedAt { get; } = createdAt;

    public static User FromUserDto(UserDto dto)
    {
        return new User(dto.Id, new Email(dto.Email), new Username(dto.Username), dto.PasswordHash, dto.CreatedAt);
    }
}