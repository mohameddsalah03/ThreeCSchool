using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Core.Domain.Specifications.Lessons
{
    public class LessonByCourseSpec : BaseSpecifications<Lesson, int>
    {
        public LessonByCourseSpec(int courseId)
            : base(l => l.CourseId == courseId && !l.IsDeleted)
        {
            AddOrderBy(l => l.DisplayOrder);
        }
    }

    public class LessonByIdAndCourseSpec : BaseSpecifications<Lesson, int>
    {
        public LessonByIdAndCourseSpec(int courseId, int lessonId)
            : base(l => l.Id == lessonId && l.CourseId == courseId && !l.IsDeleted)
        {
        }
    }
}
