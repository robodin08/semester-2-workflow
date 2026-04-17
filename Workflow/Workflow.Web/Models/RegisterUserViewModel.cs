using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class RegisterUserViewModel
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, underscore.")]
    public string Username { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(64, MinimumLength = 8)]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_\-+=\[\]{};:'"",.<>?/|\\~`]+$",
        ErrorMessage = "Password contains invalid characters.")]
    public string Password { get; init; } = string.Empty;
}

