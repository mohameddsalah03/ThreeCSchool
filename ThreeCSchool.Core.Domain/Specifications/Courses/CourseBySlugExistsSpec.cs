using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Core.Domain.Specifications.Courses
{
    public class CourseBySlugExistsSpec : BaseSpecifications<Course, int>
    {
        public CourseBySlugExistsSpec(string slug)
            : base(c => c.Slug == slug && !c.IsDeleted)
        {
        }
    }
}
