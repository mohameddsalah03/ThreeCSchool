using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        [RegularExpression(
            "^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$%&*()_+\\-={}\\[\\]|;:,.<>?/~])[A-Za-z\\d@#$%&*()_+\\-={}\\[\\]|;:,.<>?/~]{6,}$",
            ErrorMessage = "Password must have 1 uppercase, 1 lowercase, 1 digit, 1 special char, min 6 chars.")]
        public required string NewPassword { get; set; }
    }
}