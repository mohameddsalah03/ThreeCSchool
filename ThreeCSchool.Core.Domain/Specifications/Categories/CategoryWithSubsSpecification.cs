using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Core.Domain.Specifications.Categories
{
    public class CategoryWithSubsSpecification : BaseSpecifications<Category, int>
    {
        // كل الـ top-level النشطة للقائمة التنقلية
        public CategoryWithSubsSpecification()
        {
            Criteria = c => c.IsActive && c.ParentCategoryId == null;
            Includes.Add(c => c.SubCategories);
            AddOrderBy(c => c.DisplayOrder);
        }

        // category واحدة بالـ slug
        public CategoryWithSubsSpecification(string slug)
        {
            Criteria = c => c.Slug == slug && c.IsActive;
            Includes.Add(c => c.SubCategories);
        }

        // category واحدة بالـ ID — للـ Admin
        public CategoryWithSubsSpecification(int id, bool forAdmin = false)
            : base(id)
        {
            Includes.Add(c => c.SubCategories);
        }
    }
}