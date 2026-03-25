using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Core.Domain.Specifications.Courses
{
    public class CourseByCategorySpec : BaseSpecifications<Course, int>
    {
        public CourseByCategorySpec(string categorySlug, int page, int pageSize)
            : base(c => c.Category.Slug == categorySlug && !c.IsDeleted && c.IsPublished)
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Instructor);
            AddOrderByDesc(c => c.CreatedAt);
            AddPagination((page - 1) * pageSize, pageSize);
        }
    }

    public class CourseByCategoryCountSpec : BaseSpecifications<Course, int>
    {
        public CourseByCategoryCountSpec(string categorySlug)
            : base(c => c.Category.Slug == categorySlug && !c.IsDeleted && c.IsPublished)
        {
        }
    }
}
