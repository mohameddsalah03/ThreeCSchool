using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeCSchool.Shared.DTOs.Lessons
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public string? ContentText { get; set; }
        public int? DurationInMinutes { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFreePreview { get; set; }
    }
}
