using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; init; } = string.Empty;

    [Required]
    [StringLength(64, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_\-+=\[\]{};:'"",.<>?/|\\~`]+$",
        ErrorMessage = "Password contains invalid characters.")]
    public string NewPassword { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; init; } = string.Empty;
}
