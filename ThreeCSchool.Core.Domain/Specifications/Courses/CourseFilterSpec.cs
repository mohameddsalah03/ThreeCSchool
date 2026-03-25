using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Shared.DTOs.Courses;

namespace ThreeCSchool.Core.Domain.Specifications.Courses
{
    public class CourseFilterSpec : BaseSpecifications<Course, int>
    {
        public CourseFilterSpec(CourseFilterDto filter)
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Instructor);

            Criteria = c =>
                !c.IsDeleted &&
                (filter.Search == null ||
                    c.TitleEn.Contains(filter.Search) ||
                    c.TitleAr.Contains(filter.Search)) &&
                (filter.CategoryId == null || c.CategoryId == filter.CategoryId) &&
                (filter.IsFree == null || c.IsFree == filter.IsFree) &&
                (filter.IsPublished == null || c.IsPublished == filter.IsPublished) &&
                (filter.MinPrice == null || c.Price >= filter.MinPrice) &&
                (filter.MaxPrice == null || c.Price <= filter.MaxPrice);

            AddOrderByDesc(c => c.CreatedAt);
            AddPagination((filter.Page - 1) * filter.PageSize, filter.PageSize);
        }
    }

    // نفس الـ Criteria بس من غير pagination — للـ Count
    public class CourseFilterCountSpec : BaseSpecifications<Course, int>
    {
        public CourseFilterCountSpec(CourseFilterDto filter)
        {
            Criteria = c =>
                !c.IsDeleted &&
                (filter.Search == null ||
                    c.TitleEn.Contains(filter.Search) ||
                    c.TitleAr.Contains(filter.Search)) &&
                (filter.CategoryId == null || c.CategoryId == filter.CategoryId) &&
                (filter.IsFree == null || c.IsFree == filter.IsFree) &&
                (filter.IsPublished == null || c.IsPublished == filter.IsPublished) &&
                (filter.MinPrice == null || c.Price >= filter.MinPrice) &&
                (filter.MaxPrice == null || c.Price <= filter.MaxPrice);
        }
    }
}
