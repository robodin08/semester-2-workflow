namespace Workflow.Core.Users;

public class CreateUserRequest(string email, string username, string password)
{
    public string Email { get; } = email;
    
    public string Username { get; } = username;
    
    public string Password { get; } = password;
}