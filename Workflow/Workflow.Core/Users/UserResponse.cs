using Data.Users;

namespace Workflow.Core.Users;

public class UserResponse(int id, string email, string username, DateTime createdAt)
{
    
    public int Id { get; } = id;
    public string Email { get; } = email;
    public string Username { get; } = username;
    public DateTime CreatedAt { get; } = createdAt;

    public static UserResponse FromUser(User user)
    {
        return new UserResponse(user.Id, user.Email.Value, user.Username.Value, user.CreatedAt);
    }
}