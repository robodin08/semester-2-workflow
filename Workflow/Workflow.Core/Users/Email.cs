using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public sealed record Email
{
    private const int MaxLength = 254;
    
    public string Value { get; }

    public Email(string value)
    {
        ValidateEmail(value);
        Value = value;
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidEmailException("Email cannot be empty");

        if (email.Length > MaxLength)
            throw new InvalidEmailException($"Email must be less than {MaxLength} characters.");

        if (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@'))
            throw new InvalidEmailException("Email format is invalid.");
    }

    public override string ToString() => Value;
}