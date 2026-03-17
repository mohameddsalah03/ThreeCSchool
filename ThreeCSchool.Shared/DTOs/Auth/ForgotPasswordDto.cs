using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}