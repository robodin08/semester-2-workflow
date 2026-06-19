using Data.Users;

namespace Workflow.Core.Users;

public class UserResponse(int id, string email, string username, string role, DateTime createdAt)
{
    
    public int Id { get; } = id;
    public string Email { get; } = email;
    public string Username { get; } = username;
    public string Role { get; } = role;
    public DateTime CreatedAt { get; } = createdAt;

    public static UserResponse FromUser(User user)
    {
        return new UserResponse(user.Id, user.Email.Value, user.Username.Value, user.Role, user.CreatedAt);
    }
}