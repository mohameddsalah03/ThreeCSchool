using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class VerifyOtpDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6,
            ErrorMessage = "OTP must be exactly 6 digits.")]
        public required string OtpCode { get; set; }
    }
}