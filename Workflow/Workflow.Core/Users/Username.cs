using System.Text.RegularExpressions;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public sealed partial record Username
{
    private const int MinLength = 2;
    private const int MaxLength = 20;

    public string Value { get; }

    public Username(string value)
    {
        ValidateUsername(value);
        Value = value;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_.]+$")]
    private static partial Regex UsernameRegex();

    private static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidUsernameException("Username cannot be empty.");

        if (username.Length is < MinLength or > MaxLength)
            throw new InvalidUsernameException(
                $"Username must be between {MinLength} and {MaxLength} characters.");

        if (!UsernameRegex().IsMatch(username))
            throw new InvalidUsernameException("Username contains invalid characters.");
    }

    public override string ToString() => Value;
}