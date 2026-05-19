using System.Net.Mail;
using System.Text.RegularExpressions;
using Data.Users;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public sealed partial class User
{
    private readonly string _passwordHash;
    
    public int Id { get; }
    public string Username { get; }
    public string Email { get; }
    public DateTime CreatedAt { get; }

    private User(int id, string username, string email, string passwordHash, DateTime createdAt)
    {
        Id = id;
        Username = username;
        Email = email;
        _passwordHash = passwordHash;
        CreatedAt = createdAt;
    }
    
    public static User FromUserDto(UserDto dto)
    {
        return new User(dto.Id, dto.Username, dto.Email, dto.PasswordHash, dto.CreatedAt);
    }
    
    public UserDto ToUserDto()
    {
        return new UserDto(Id, Email, _passwordHash, Username, CreatedAt);
    }
}