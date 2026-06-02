using System.Net.Mail;
using System.Text.RegularExpressions;
using Data.Users;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public class User(int id, Email email, Username username, DateTime createdAt)
{
    
    public int Id { get; } = id;
    public Email Email { get; } = email;
    public Username Username { get; } = username;
    public DateTime CreatedAt { get; } = createdAt;

    public static User FromUserDto(UserDto dto)
    {
        return new User(dto.Id, new Email(dto.Email), new Username(dto.Username), dto.CreatedAt);
    }
}