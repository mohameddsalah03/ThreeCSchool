using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Categories
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(150)]
        public required string NameEn { get; set; }

        [Required]
        [MaxLength(150)]
        public required string NameAr { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Slug { get; set; }

        [MaxLength(500)]
        public string? IconUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // null = top-level | int = subcategory
        public int? ParentCategoryId { get; set; }
    }
}