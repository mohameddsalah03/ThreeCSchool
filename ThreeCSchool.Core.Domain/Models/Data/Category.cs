using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Models.Data
{
    public class Category : BaseEntity<int>
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? IconUrl { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Self-referencing FK
        public int? ParentCategoryId { get; set; }

        // Navigation Properties
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}