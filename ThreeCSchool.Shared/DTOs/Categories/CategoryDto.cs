namespace ThreeCSchool.Shared.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? IconUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<SubCategoryDto> SubCategories { get; set; } = new();
    }
}