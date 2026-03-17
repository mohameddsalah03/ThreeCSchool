using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class ChangePasswordDto
    {
        [Required]
        public required string CurrentPassword { get; set; }

        [Required]
        [RegularExpression(
            "^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$%&*()_+\\-={}\\[\\]|;:,.<>?/~])[A-Za-z\\d@#$%&*()_+\\-={}\\[\\]|;:,.<>?/~]{6,}$",
            ErrorMessage = "Password must have 1 uppercase, 1 lowercase, 1 digit, 1 special char, min 6 chars.")]
        public required string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public required string ConfirmNewPassword { get; set; }
    }
}