using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class Lesson : BaseEntity<int>
    {
        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public string? ContentText { get; set; }
        public int? DurationInMinutes { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsFreePreview { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime UpdatedAt { get; set; } = DateTime.MinValue;

        // FK
        public int CourseId { get; set; }

        // Navigation
        public Course Course { get; set; } = null!;
    }
}