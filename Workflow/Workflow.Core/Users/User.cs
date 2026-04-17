using Data.Users;

namespace Workflow.Core.Users;

public class User(int userId, string username, string email, DateTime createdAt)
{
    public int Id { get; } = userId;
    public string Username { get; } = username;
    public string Email { get; } = email;
    public DateTime CreatedAt { get; } = createdAt;

    public static User FromUserModel(UserModel user)
    {
        return new User(user.Id, user.Username, user.Email, user.CreatedAt);
    }
}