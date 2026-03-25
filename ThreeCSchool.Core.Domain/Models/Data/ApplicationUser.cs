using Microsoft.AspNetCore.Identity;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public string TimeZone { get; set; } = "Africa/Cairo";
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // OTP
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

        // Navigation
        public Cart? Cart { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();

        //  أضفنا — الـ Instructor بيكون assigned لـ Sessions
        public ICollection<CourseSession> SessionsAsInstructor { get; set; } = new List<CourseSession>();

        //  أضفنا — الـ Student بيحجز Sessions
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

