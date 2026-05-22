namespace Workflow.Core.Users;

public class RegisterRequest(string email, string username, string password)
{
    public string Email { get; init; } = email;
    public string Username { get; init; } = username;
    public string Password { get; init; } = password;
}