using System.Net.Mail;
using System.Text.RegularExpressions;
using Data.Users;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public sealed partial class User
{
    
    public int Id { get; }
    public string Email { get; }
    public string Username { get; }
    public DateTime CreatedAt { get; }

    private User(int id, string email, string username, DateTime createdAt)
    {
        Id = id;
        Email = email;
        Username = username;
        CreatedAt = createdAt;
    }
    
    public static User FromUserDto(UserDto dto)
    {
        return new User(dto.Id, dto.Email, dto.Username, dto.CreatedAt);
    }
}