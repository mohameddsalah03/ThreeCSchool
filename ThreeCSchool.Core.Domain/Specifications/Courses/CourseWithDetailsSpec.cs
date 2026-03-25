using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Core.Domain.Specifications.Courses
{
    public class CourseWithDetailsSpec : BaseSpecifications<Course, int>
    {
        // Single by slug
        public CourseWithDetailsSpec(string slug)
            : base(c => c.Slug == slug && !c.IsDeleted)
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Instructor);
        }

        // Single by id
        public CourseWithDetailsSpec(int id)
            : base(c => c.Id == id && !c.IsDeleted)
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Instructor);
        }
    }
}
