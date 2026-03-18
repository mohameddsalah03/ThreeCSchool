namespace ThreeCSchool.Shared.DTOs.Categories
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? IconUrl { get; set; }
        public int DisplayOrder { get; set; }
    }
}