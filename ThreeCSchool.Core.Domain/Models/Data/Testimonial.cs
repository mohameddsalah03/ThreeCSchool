using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class Testimonial : BaseEntity<int>
    {
        public string PersonName { get; set; } = null!;
        public string PersonType { get; set; } = null!; // "Student" or "Parent"
        public string YoutubeVideoId { get; set; } = null!;
        public string? TitleEn { get; set; }
        public string? TitleAr { get; set; }
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
    }
}