using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class Review : BaseEntity<int>
    {
        public string UserId { get; set; } = null!;
        public int CourseId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ApplicationUser User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}