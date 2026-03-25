using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeCSchool.Shared.DTOs.Courses
{
    public class UpdateCourseDto
    {
        public string? TitleEn { get; set; }
        public string? TitleAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? Type { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool? IsDiscount { get; set; }
        public bool? IsFree { get; set; }
        public bool? IsDownloadable { get; set; }
        public bool? IsUpcoming { get; set; }
        public int? CategoryId { get; set; }
    }
}
