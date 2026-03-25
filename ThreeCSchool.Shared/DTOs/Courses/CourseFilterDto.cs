using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeCSchool.Shared.DTOs.Courses
{
    public class CourseFilterDto
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsFree { get; set; }
        public bool? IsPublished { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
