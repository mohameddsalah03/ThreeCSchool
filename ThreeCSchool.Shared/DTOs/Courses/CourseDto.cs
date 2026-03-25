using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeCSchool.Shared.DTOs.Courses
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Type { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }
        public bool IsFree { get; set; }
        public bool IsDownloadable { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsPublished { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalEnrollments { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; } = null!;
        public string InstructorName { get; set; } = null!;
    }
}
