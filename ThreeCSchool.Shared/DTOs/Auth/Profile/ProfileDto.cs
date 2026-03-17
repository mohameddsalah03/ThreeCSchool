namespace ThreeCSchool.Shared.DTOs.Auth.Profile
{
    public class ProfileDto
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string TimeZone { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEmailVerified { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}