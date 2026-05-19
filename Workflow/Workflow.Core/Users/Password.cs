using System.Text.RegularExpressions;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public sealed partial record Password
{
    private const int MinLength = 8;
    private const int MaxLength = 64;

    public string Value { get; }

    public Password(string value)
    {
        ValidatePassword(value);
        Value = value;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9!@#$%^&*()_\-+=\[\]{};:'"",.<>?/|\\~`]+$")]
    private static partial Regex PasswordRegex();

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidPasswordException("password cannot be empty.");

        if (password.Length is < MinLength or > MaxLength)
            throw new InvalidPasswordException(
                $"Password must be between {MinLength} and {MaxLength} characters.");

        if (!PasswordRegex().IsMatch(password))
            throw new InvalidPasswordException(
                "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
    }

    public override string ToString() => Value;
}