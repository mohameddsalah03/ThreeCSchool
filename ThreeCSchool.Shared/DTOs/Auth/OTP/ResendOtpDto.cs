using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth.OTP
{
    public class ResendOtpDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}