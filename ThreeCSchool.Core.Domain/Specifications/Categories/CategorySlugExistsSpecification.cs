using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Core.Domain.Specifications.Categories
{
    public class CategorySlugExistsSpecification : BaseSpecifications<Category, int>
    {
        // فحص تكرار الـ Slug عند الإنشاء
        public CategorySlugExistsSpecification(string slug)
        {
            Criteria = c => c.Slug == slug;
        }

        // فحص تكرار الـ Slug عند التعديل — بنستثني الـ ID الحالي
        public CategorySlugExistsSpecification(string slug, int excludeId)
        {
            Criteria = c => c.Slug == slug && c.Id != excludeId;
        }
    }
}