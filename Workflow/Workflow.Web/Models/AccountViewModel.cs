using Workflow.Core.Users;

namespace Web.Models;

public class AccountViewModel
{
    public string Email { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public ChangePasswordViewModel ChangePasswordForm { get; init; } = new();
}
