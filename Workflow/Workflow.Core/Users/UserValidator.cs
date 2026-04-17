using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Workflow.Core.Users;

public static partial class UserValidator
{
    [GeneratedRegex(@"^[a-zA-Z0-9_.]+$")]
    private static partial Regex UsernameRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9!@#$%^&*()_\-+=\[\]{};:'"",.<>?/|\\~`]+$")]
    private static partial Regex PasswordRegex();

    public static void ValidateCreateUserRequest(CreateUserRequest request)
    {
        ValidateEmail(request.Email);
        ValidateUsername(request.Username);
        ValidatePassword(request.Password);
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
            case > 20:
                throw new ArgumentException("Username cannot be longer than 20 characters");
        }

        if (!UsernameRegex().IsMatch(username))
            throw new ArgumentException("Username contains invalid characters");
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty");

        switch (password.Length)
        {
            case < 8:
                throw new ArgumentException("Password must be at least 8 characters");
            case > 64:
                throw new ArgumentException("Password cannot be longer than 64 characters");
        }

        if (!PasswordRegex().IsMatch(password))
            throw new ArgumentException("Password contains invalid characters");
    }
}