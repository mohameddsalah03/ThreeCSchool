using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(150)]
        public required string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        [RegularExpression(
            "^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$%&*()_+\\-={}\\[\\]|;:,.<>?/~])[A-Za-z\\d@#$%&*()_+\\-={}\\[\\]|;:,.<>?/~]{6,}$",
            ErrorMessage = "Password must have 1 uppercase, 1 lowercase, 1 digit, 1 special char, min 6 chars.")]
        public required string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public required string RetypePassword { get; set; }

        [Required]
        public required string Role { get; set; } // Student | Instructor | Organization

        [Required]
        public required string TimeZone { get; set; }
    }
}