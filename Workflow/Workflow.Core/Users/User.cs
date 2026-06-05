using Data.Users;

namespace Workflow.Core.Users;

public class User(int id, Email email, Username username, DateTime createdAt)
{
    
    public int Id { get; } = id;
    public Email Email { get; } = email;
    public Username Username { get; } = username;
    public DateTime CreatedAt { get; } = createdAt;

    public static User FromUserDto(UserModel model)
    {
        return new User(model.Id, new Email(model.Email), new Username(model.Username), model.CreatedAt);
    }
}