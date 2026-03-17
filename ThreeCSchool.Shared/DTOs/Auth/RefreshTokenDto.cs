using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public required string Token { get; set; }

        [Required]
        public required string RefreshToken { get; set; }
    }
}