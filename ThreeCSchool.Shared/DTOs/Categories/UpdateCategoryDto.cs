using System.ComponentModel.DataAnnotations;

namespace ThreeCSchool.Shared.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        [MaxLength(150)]
        public string? NameEn { get; set; }

        [MaxLength(150)]
        public string? NameAr { get; set; }

        [MaxLength(200)]
        public string? Slug { get; set; }

        [MaxLength(500)]
        public string? IconUrl { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsActive { get; set; }
    }
}