using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class LoginByPhoneDto
    {
        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}