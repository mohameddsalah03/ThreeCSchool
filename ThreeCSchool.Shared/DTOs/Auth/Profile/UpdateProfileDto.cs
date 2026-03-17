using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Auth.Profile
{
    public class UpdateProfileDto
    {
        [MaxLength(150)]
        public string? DisplayName { get; set; }

        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? ProfilePicture { get; set; }

        [MaxLength(100)]
        public string? TimeZone { get; set; }
    }
}