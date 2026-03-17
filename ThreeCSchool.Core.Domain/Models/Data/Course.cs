using ThreeCSchool.Core.Domain.Models.Base;
using ThreeCSchool.Core.Domain.Models.Data.Enums;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class Course : BaseEntity<int>
    {
        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? ThumbnailUrl { get; set; }
        public CourseType Type { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsDiscount { get; set; } = false;
        public bool IsFree { get; set; } = false;
        public bool IsDownloadable { get; set; } = false;
        public bool IsUpcoming { get; set; } = false;
        public bool IsPublished { get; set; } = false;
        public decimal AverageRating { get; set; } = 0;
        public int TotalEnrollments { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // FKs
        public int CategoryId { get; set; }
        public string InstructorId { get; set; } = null!;

        // Navigation Properties
        public Category Category { get; set; } = null!;
        public ApplicationUser Instructor { get; set; } = null!;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}