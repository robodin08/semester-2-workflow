namespace Data.Users;

<<<<<<<< HEAD:Workflow/Workflow.Data/Users/CreateUserDto.cs
public class CreateUserDto(string email, string username, string passwordHash)
========
public class RegisterDto(string email, string username, string passwordHash)
>>>>>>>> origin/User-Login:Workflow/Workflow.Data/Users/RegisterDto.cs
{
    public string Email { get; } = email;
    public string Username { get; } = username;
    public string PasswordHash { get; } = passwordHash;
}