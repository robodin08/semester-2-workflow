using System.Net.Mail;

namespace Workflow.Core.Users;

public class CreateUserRequest(string email, string username, string password)
{
    public string Email { get; } = email;
    
    public string Username { get; } = username;
    
    public string Password { get; } = password;
    
    public void Validate()
    {
        ValidateEmail(Email);
        ValidateUsername(Username);
        ValidatePassword(Password);
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        if (email.Length > 254)
            throw new ArgumentException("Email is too long");
        
        try
        {
            var addr = new MailAddress(email);
            if (addr.Address != email)
            {
                throw new ArgumentException("Email format is invalid");
            }
        }
        catch
        {
            throw new ArgumentException("Email format is invalid");
        }
    }

    private static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty");

        switch (username.Length)
        {
            case < 3:
                throw new ArgumentException("Username must be at least 3 characters");
            case > 50:
                throw new ArgumentException("Username cannot be longer than 50 characters");
        }
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty");

        switch (password.Length)
        {
            case < 8:
                throw new ArgumentException("Password must be at least 8 characters");
            case > 100:
                throw new ArgumentException("Password cannot be longer than 100 characters");
        }
    }
}